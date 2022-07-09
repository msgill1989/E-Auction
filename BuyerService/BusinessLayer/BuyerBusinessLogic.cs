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
                ValidateDateRequest dateToValidate = new ValidateDateRequest() { ProductId = bidDetails.ProductId, BidDate = DateTime.Now};
                //If bid is placed after the bid end date. throw a exception. -----------To be implemented--------------------
                await TopicMessagePublisher("BuyerProducer", "isBidDateValid", JsonConvert.SerializeObject(dateToValidate));

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
        public async Task UpdateBid(string bidId, double bidAmount)
        {
            await _buyerRepository.UpdateBid(bidId, bidAmount);
        }

        public async Task TopicMessageListener()
        {
            try
            {
                using (var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
                {
                    consumer.Subscribe("SellerProducer");
                    while (true)
                    { 
                        var msg = consumer.Consume();
                        if (msg != null)
                        {
                            switch (msg.Message.Key)
                            {
                                case "checkBidDetails":
                                    await IsBidPresentForProductId(msg.Message.Value);
                                    msg = null;
                                break;
                                case "bidDateResponse":

                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task IsBidPresentForProductId(string productId)
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
                await TopicMessagePublisher("BuyerProducer","isBidPresent", serializedResponse);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task TopicMessagePublisher(string topic,string key, string value)
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
    }
}
