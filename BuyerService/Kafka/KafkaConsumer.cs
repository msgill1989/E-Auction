using BuyerService.BusinessLayer.Interfaces;
using BuyerService.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BuyerService.Kafka
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IBuyerBusinessLogic _buyerBusinessLayer;
        private readonly ILogger<KafkaConsumer> _logger;
        public KafkaConsumer(ConsumerConfig consumerConfig, IBuyerBusinessLogic buyerBusinesslayer, ILogger<KafkaConsumer> logger)
        {
            _consumerConfig = consumerConfig;
            _buyerBusinessLayer = buyerBusinesslayer;
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
                        consumer.Subscribe("SellerProducer");
                        
                        var msg = consumer.Consume();
                        if (msg != null)
                        {
                            switch (msg.Message.Key)
                            {
                                case "checkBidDetails":
                                    await _buyerBusinessLayer.IsBidPresentForProductIdAsync(msg.Message.Value);
                                    msg = null;
                                    break;
                                case "isBidDateValid":
                                    var message = JsonConvert.DeserializeObject<ValidateDateResponse>(msg.Message.Value);
                                    _buyerBusinessLayer.CollateResponseForQueue(message.Operation, message.productId, message.IsValid);
                                    msg = null;
                                    break;
                                case "GetAllBids":
                                    await _buyerBusinessLayer.GetAllBidDetailsAsync(msg.Message.Value);
                                    msg = null;
                                    break;
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
