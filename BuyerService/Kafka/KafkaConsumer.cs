using BuyerService.BusinessLayer.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerService.Kafka
{
    public class KafkaConsumer
    {
        public KafkaConsumer(ConsumerConfig consumerConfig, IBuyerBusinessLogic buyerBusinesslayer, ILogger<KafkaConsumer> logger)
        {

        }
    }
}
