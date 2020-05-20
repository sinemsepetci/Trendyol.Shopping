using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trendyol.Shopping.Entity.Concrete;

namespace Trendyol.Shopping
{
    public class RabbitMQClient
    {
        public void Push(List<CartItem> cartItems)
        {
            Thread.Sleep(1000);
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                foreach (var item in cartItems)
                {
                    channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);
                    //channel.QueueDeclare(queue: item.Id,
                    //                 durable: false,
                    //                 exclusive: false,
                    //                 autoDelete: false,
                    //                 arguments: null);

                    string message = JsonConvert.SerializeObject(item);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "direct_logs",
                                         routingKey: item.Id,
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($"Gönderilen cart: {message}");

                }
                channel.Close();
                connection.Close();
            }

            Console.WriteLine(" İlgili sepet gönderildi...");

        }

        public List<CartItem> Pop(string Id)
        {
            Thread.Sleep(1000);
            var cartItems = new List<CartItem>();
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);
                channel.QueueDeclare(Id, false, false, true, null);
                var queueCount = channel.MessageCount(Id);

                var consumer = new EventingBasicConsumer(channel);
                for (int i = 0; i < queueCount; i++)
                {
                        Thread.Sleep(3000);

                    consumer.Received += (model, ea) =>
                    {
                        string message = Encoding.UTF8.GetString(ea.Body.ToArray());

                        var obj = JsonConvert.DeserializeObject<CartItem>(message);
                        cartItems.Add(obj);
                        Console.ReadLine();
                    };


                    channel.BasicConsume(queue: Id,
                                        autoAck: false,
                                        consumer: consumer);

                }
                channel.Close();
                connection.Close();
            }
            return cartItems;

        }

    }
}
