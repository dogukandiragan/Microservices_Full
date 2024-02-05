using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Base.Events;

namespace EventBus.Base.Abstraction
{
    public interface IIntegrationEventHandler<IIntegrationEvent> : IntegrationEventHandler where IIntegrationEvent : IntegrationEvent
    {

        Task Handle(IIntegrationEvent @event);


    }
    
    public interface IntegrationEventHandler
    {

    
    }
   

}