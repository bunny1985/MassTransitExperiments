using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using MassTransit;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BB.Bus.Test
{
    [TestFixture]
    public class TestClass
    {
        private class GuidSerializer : ISerializer<Guid>
        {
            private readonly StringSerializer _stringSerializer;

            public GuidSerializer()
            {
                _stringSerializer = new StringSerializer(Encoding.UTF8);
            }

            public byte[] Serialize(Guid data)
            {
                return _stringSerializer.Serialize(data.ToString());
            }
        }

        [TestCase]
        public async Task KafkaConnectExperiment()
        {
            var config = new Dictionary<string, object> { { "bootstrap.servers", "kafkaserver" } };
            var producer = new Producer<Guid, string>(config, new GuidSerializer(), new StringSerializer(Encoding.UTF8));
            EventHandler<Error> handler = (s, e) =>
            {
                System.Console.WriteLine(s);
                System.Console.WriteLine(e);
                Assert.Fail();
            };
            producer.OnError += handler;

            var deliveryReport = await producer.ProduceAsync("DupaTopic", Guid.NewGuid(), "SomeText");
            System.Console.WriteLine("OK");
        }
    }
}