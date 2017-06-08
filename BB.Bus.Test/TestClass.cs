using bb.bus;
using MassTransit;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BB.Bus.Test
{
    [TestFixture]
    public class TestClass
    {
        public class TestMsg : Messages.Command
        {
            public TestMsg() : base("Dupa")
            {
            }

            public string Msg { get; set; }
        }

        public class ConsoleLogEventStore : IEventsStore
        {
            public Task StoreAsync(object obj)
            {
                var str = JsonConvert.SerializeObject(obj);
                System.Console.WriteLine(str);
                return Task.CompletedTask;
            }
        }

        public class UpdateCustomerConsumer :
                IConsumer<TestMsg>
        {
            public async Task Consume(ConsumeContext<TestMsg> context)
            {
                Console.WriteLine("ODEBRANO");
            }
        }

        [Test]
        public async Task BusFullIntegrationTest()
        {
            var bus = MassTransit.Bus.Factory.CreateUsingRabitMqOnLocalhost("admin", "admin", (cfg, host) =>
            {
                cfg.ReceiveEndpoint(host, e => e.Consumer<UpdateCustomerConsumer>());
            });
            bus.RegisterEventsStore(new ConsoleLogEventStore());
            await bus.StartAsync();

            for (int i = 0; i < 10; i++)
            {
                await bus.Publish(new TestMsg { CorrelationId = i.ToString(), Msg = "ALE JAJAJA" + i });
            }

            Thread.Sleep(1000);

            await bus.StopAsync();

            Assert.Pass("Your first passing test");
        }
    }
}