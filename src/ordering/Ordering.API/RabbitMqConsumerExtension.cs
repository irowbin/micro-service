using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.API.RabbitMq;

namespace Ordering.API
{
    public static class RabbitMqConsumerExtension
    {
        public static EventBusRabbitMqConsumer Consumer { get; set; }
        
        public static IApplicationBuilder UseRabbitMqListener(this IApplicationBuilder app)
        {
            Consumer = app.ApplicationServices.GetService<EventBusRabbitMqConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            life?.ApplicationStarted.Register(() => { Consumer.Consume(); });
            life?.ApplicationStopping.Register(() => { Consumer.Disconnect(); });
            return app;
        }
    }
}