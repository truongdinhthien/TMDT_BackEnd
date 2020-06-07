using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace MessageBus.Config
{
    public class RabbitMqBus
    {
        public static IBusControl ConfigureBus(IRegistrationContext<IServiceProvider> provider, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost>
         registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(BusConstant.RabbitMqUri), hst =>
                {
                    hst.Username(BusConstant.UserName);
                    hst.Password(BusConstant.Password);
                });

                cfg.ConfigureEndpoints(provider);

                registrationAction?.Invoke(cfg, host);
            });
        }
    }
}