using bb.bus;
using MassTransit.RabbitMqTransport;
using System;

namespace MassTransit
{
    public static class BusExtentions
    {
        public static IBusControl CreateUsingRabitMqOnLocalhost(this IBusFactorySelector bus, string userName, string password, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configCallback = null)
        {
            return bus.CreateUsingSimpleRabbitMqConfiguration("rabbitmq://localhost", userName, password, configCallback);
        }

        public static void RegisterEventsStore(this IBusControl bus, IEventsStore eventsStore)
        {
            bus.ConnectSendAuditObservers(new MessageAuditStoreWithEventsStore(eventsStore));
        }

        //bus.ConnectPublishObserver(observer);
        //  bus.ConnectSendObserver(observer);
        public static IBusControl CreateUsingSimpleRabbitMqConfiguration(this IBusFactorySelector bus, string connectionString, string userName, string password, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configCallback = null)
        {
            return bus.CreateUsingRabbitMq(config =>
            {
                var host = config.Host(new Uri(connectionString), h =>
               {
                   h.Username(userName);
                   h.Password(password);
               });

                config.UseJsonSerializer();
                if (configCallback != null)
                {
                    configCallback(config, host);
                }
            });
        }
    }
}