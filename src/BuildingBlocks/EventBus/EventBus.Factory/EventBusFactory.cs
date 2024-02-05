using EventBus.Base;
using EventBus.Base.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.RabbitMQ;

namespace EventBus.Factory
{
    public static class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                _ => new EventBusRabbitMQ(config, serviceProvider),
                //EventBusType.AzureServiceBus => new EventBusServiceBus(config, serviceProvider),

                //_ => new EventBusRabbitMQ(config, serviceProvider),

            };
        }
    }
}
