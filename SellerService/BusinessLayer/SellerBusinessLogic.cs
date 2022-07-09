using SellerService.BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SellerService.RepositoryLayer.Interfaces;
using SellerService.RepositoryLayer;
using Microsoft.Extensions.Logging;
using SellerService.Models;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace SellerService.BusinessLayer
{
    public class SellerBusinessLogic : ISellerBusinessLogic
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly ILogger<SellerBusinessLogic> _logger;
        private readonly ProducerConfig _producerconfig;
        private readonly ConsumerConfig _consumerConfig;
        private readonly Dictionary<int, bool> bidResponseFromBuyer;

        public SellerBusinessLogic(ISellerRepository sellerRepository, ILogger<SellerBusinessLogic> logger, ProducerConfig producerConfig, ConsumerConfig consumerConfig)
        {
            _sellerRepository = sellerRepository;
            _logger = logger;
            _producerconfig = producerConfig;
            _consumerConfig = consumerConfig;
        }
        public async Task AddProductBLayerAsync(ProductAndSeller productObj)
        {
            await _sellerRepository.AddProductAsync(productObj);
        }

        public async Task DeleteProductBLayerAsync(string productId)
        {
            try
            {
                //Get the added product from DB
                ProductAndSeller productDetails = await _sellerRepository.GetProductAsync(productId);

                //Check the Bid end date
                if (productDetails.BidEndDate < DateTime.Now)
                    throw new KeyNotFoundException("Product cannot be deleted after the BidEnd date.");

                //If Any bid is already placed Dont delete the product
                await TopicMessagePublisher("SellerProducer", "checkBidDetails", productId);

                while (true)
                {
                    if (bidResponseFromBuyer.ContainsKey(Convert.ToInt16(productId)))
                    {
                        if (bidResponseFromBuyer.FirstOrDefault(x => x.Key == Convert.ToInt16(productId)).Value == false)
                        {
                            throw new KeyNotFoundException("Product can not be deleted because there is already a bid placed for this product.");
                        }
                        else
                            break;
                    }
                }

                //Delete the product
                await _sellerRepository.DeleteProductAsync(productId);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ShowBidsResponse> GetAllBidDetailsAsync(string productId)
        {
            try
            {
                var productDetails = await _sellerRepository.GetProductAsync(productId);

                //Get the Bid Details

                return new ShowBidsResponse();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task TopicMessagePublisher(string topic,string key, string value)
        {
            try
            {
                using (var producer = new ProducerBuilder<string, string>(_producerconfig).Build())
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

        public async Task TopicMessageListener()
        {
            try
            {
                using (var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
                {
                    consumer.Subscribe("BuyerProducer");
                    while (true)
                    {
                        var msg = consumer.Consume();
                        if (msg != null)
                        {
                            switch (msg.Message.Key)
                            {
                                case "isBidPresent":
                                    await IsBidPresentForProductId(msg.Message.Value);
                                    msg = null;
                                    break;
                                case "isBidDateValid":
                                    var request = JsonConvert.DeserializeObject<ValidateDateRequest>(msg.Message.Value);
                                    await IsBidDateValid(request);
                                    msg = null;
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

        public async Task IsBidPresentForProductId(string message)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<IsBidPresentResponse>(message);
                bidResponseFromBuyer.Add(response.ProductId, response.IsBidPresent);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task IsBidDateValid(ValidateDateRequest request)
        {
            try
            {
                var productDetails = await _sellerRepository.GetProductAsync(request.ProductId);
                if(request.BidDate > productDetails.BidEndDate)

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
