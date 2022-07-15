using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SellerService.BusinessLayer.Interfaces;
using SellerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SellerService.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ISellerBusinessLogic _sellerBusinessLogic;
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(ConsumerConfig consumerconfig, ISellerBusinessLogic sellerBusinessLogic, ILogger<KafkaConsumer> logger)
        {
            _consumerConfig = consumerconfig;
            _sellerBusinessLogic = sellerBusinessLogic;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => StartConsumer(stoppingToken));
            return Task.CompletedTask;
        }

        private async Task StartConsumer(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
                    {
                        consumer.Subscribe("BuyerProducer");
                        var msg = consumer.Consume();
                        if (msg != null)
                        {
                            switch (msg.Message.Key)
                            {
                                case "isBidPresent":
                                    _sellerBusinessLogic.IsBidPresentForProductId(msg.Message.Value);
                                    msg = null;
                                    break;
                                case "isBidDateValid":
                                    var request = JsonConvert.DeserializeObject<ValidateDateRequest>(msg.Message.Value);
                                    await _sellerBusinessLogic.IsBidDateValidAsync(request);
                                    msg = null;
                                    break;
                                case "bidList":
                                    var bids = JsonConvert.DeserializeObject<GetAllBidDetailsResponse>(msg.Message.Value);
                                    _sellerBusinessLogic.CollateBidsResponse(bids.ProductId, bids.Bids);
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
    }
}
