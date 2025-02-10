using Ambev.Domain.Base;

namespace Ambev.Domain.Interfaces.Integrations;

public interface IRabbitMQIntegration
{
    public Task PublishMessageAsync(BaseEvent @event);
}