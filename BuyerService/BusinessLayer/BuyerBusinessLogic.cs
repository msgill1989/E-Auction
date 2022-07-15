using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyerService.BusinessLayer.Interfaces;
using BuyerService.Models;
using BuyerService.RepositoryLayer.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BuyerService.BusinessLayer
{
    public class BuyerBusinessLogic : IBuyerBusinessLogic
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly ILogger<BuyerBusinessLogic> _logger;
        private readonly ConsumerConfig _consumerConfig;
        private readonly ProducerConfig _producerConfig;
        private readonly static Dictionary<string, bool> validDateResponseQueueForADD = new Dictionary<string, bool>() ;
        private readonly static Dictionary<string, bool> validDateResponseQueueForUPDATE = new Dictionary<string, bool>();
        public BuyerBusinessLogic(IBuyerRepository buyerRepository, ILogger<BuyerBusinessLogic> logger, ConsumerConfig consumerConfig, ProducerConfig producerConfig)
        {
            _buyerRepository = buyerRepository;
            _logger = logger;
            _consumerConfig = consumerConfig;
            _producerConfig = producerConfig;
        }

        public async Task AddBid(BidAndBuyer bidDetails)
        {
            try
            {
                ValidateDateRequest dateToValidate = new ValidateDateRequest() { ProductId = bidDetails.ProductId, Operation = "Add", BidDate = DateTime.Now};
                //If bid is placed after the bid end date. throw a exception.
                await TopicMessagePublisherAsync("BuyerProducer", "isBidDateValid", JsonConvert.SerializeObject(dateToValidate));

                while (true)
                {
                    if (validDateResponseQueueForADD.ContainsKey(bidDetails.ProductId))
                    {
                        if (validDateResponseQueueForADD.FirstOrDefault(x => x.Key == bidDetails.ProductId).Value == false)
                        {
                            validDateResponseQueueForADD.Remove(bidDetails.ProductId);
                            throw new KeyNotFoundException("Bid cannot be placed after bid end date");
                        }
                        else
                        {
                            validDateResponseQueueForADD.Remove(bidDetails.ProductId);
                            break;
                        }
                    }
                }

                //Check if the same user has already placed a bid
                BidAndBuyer existingBid = await _buyerRepository.GetBidDetails(bidDetails.ProductId, bidDetails.Email);
                if (existingBid != null)
                    throw new KeyNotFoundException("This buyer has already placed bid for this product.");

                await _buyerRepository.AddBid(bidDetails);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public async Task UpdateBid(string productId, string buyerEmailId, double bidAmount)
        {
            await TopicMessagePublisherAsync("BuyerProducer", "isBidDateValidForUpdate", productId);

            while (true)
            {
                if (validDateResponseQueueForUPDATE.ContainsKey(productId))
                {
                    if (validDateResponseQueueForUPDATE.FirstOrDefault(x => x.Key == productId).Value == false)
                    {
                        throw new KeyNotFoundException("Bid cannot be updated after bid end date.");
                    }
                    else
                        break;
                }
            }
            await _buyerRepository.UpdateBid(productId, buyerEmailId, bidAmount);
        }

        public async Task IsBidPresentForProductIdAsync(string productId)
        {
            try
            {
                bool response;
                var BidDetails = _buyerRepository.GetBidDetails(productId);
                if (BidDetails == null)
                    response = false;
                else
                    response =  true;
                var serializedResponse = JsonConvert.SerializeObject(new IsBidPresentResponse() { ProductId = Convert.ToInt32(productId), IsBidPresent = response });
                await TopicMessagePublisherAsync("BuyerProducer","isBidPresent", serializedResponse);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task TopicMessagePublisherAsync(string topic,string key, string value)
        {
            try
            {
                using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
                {
                    await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = value });
                    producer.Flush(TimeSpan.FromSeconds(10));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task GetAllBidDetailsAsync(string productId)
        {
            try
            {
                GetAllBidsResponse response = new GetAllBidsResponse();
                response.ProductId = productId;
                List<BidDetails> bidLst = new List<BidDetails>();
                var bidDetails = _buyerRepository.GetAllBidsForProductId(productId);
                foreach (var bid in bidDetails)
                {
                    bidLst.Add(new BidDetails { BidAmount = bid.BidAmount, BuyerFName = bid.BuyerFName, Email = bid.Email, Mobile = bid.Phone });
                }
                response.Bids.AddRange(bidLst);
                var serializedBids = JsonConvert.SerializeObject(response);
                await TopicMessagePublisherAsync("ButerProducer", "bidList", serializedBids);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CollateResponseForQueue(string operation, string productId, bool isValid)
        {
            try
            {
                if (operation == "ADD")
                {
                    validDateResponseQueueForADD.Add(productId, isValid);
                }
                else if (operation == "UPDATE")
                {
                    validDateResponseQueueForUPDATE.Add(productId, isValid);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
