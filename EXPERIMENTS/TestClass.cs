using BusDomain.CommandHalders;
using BusDomain.Commnads;
using BusDomain.Events;
using BusDomain.Identities;
using EventFlow;
using EventFlow.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EXPERIMENTS
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public async Task CommandShouldBehandledByHandler()
        {
            var uri = new Uri("amqp://admin:admin@localhost/mainVHost");
            using (var resolver = EventFlowOptions.New.PublishToRabbitMq(RabbitMqConfiguration.With(uri))
                    .AddCommands(typeof(PublishMessageCommand))
                    .AddEvents(typeof(MessagePublishedEvent))
                    .AddCommandHandlers(typeof(PublishMessageCommandHandler))

                    .CreateResolver())
            {
                // Create a new identity for our aggregate root
                var exampleId = BasicIdentity.New;
                // Resolve the command bus and use it to publish a command
                var commandBus = resolver.Resolve<ICommandBus>();
                for (int x = 0; x < 1000; x++)
                {
                    await commandBus.PublishAsync(new PublishMessageCommand(BasicIdentity.New, "Dupa 2"), CancellationToken.None).ConfigureAwait(false);
                }
            }
        }
    }
}