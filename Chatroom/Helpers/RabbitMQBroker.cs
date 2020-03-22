using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace Chatroom.Helpers
{
    public class RabbitMQBroker : IMessageBroker
    {
        private string rabbutmqQueu;
        private IConnection connection;

        public RabbitMQBroker(string queu, ConnectionFactory factory)
        {
            this.rabbutmqQueu = queu;
            this.connection = factory.CreateConnection();
        }

        public bool send(string message)
        {
            try
            {
                IModel channel = this.connection.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(this.rabbutmqQueu, true, false, false, null);
                channel.QueueBind(this.rabbutmqQueu, "messageexchange", this.rabbutmqQueu, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("messageexchange", this.rabbutmqQueu, null, msg);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string receive()
        {
            try
            {
                IModel channel = this.connection.CreateModel();
                channel.QueueDeclare(queue: this.rabbutmqQueu, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: this.rabbutmqQueu, autoAck: true);
                return (result != null) ? Encoding.UTF8.GetString(result.Body) : String.Empty;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        public bool suscribe()
        {
            try
            {
                IModel channel = this.connection.CreateModel();
                channel.QueueDeclare(queue: this.rabbutmqQueu, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queue: this.rabbutmqQueu, autoAck: true, consumer: consumer);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            NotificationHelper.sendMessageToAllClients(Encoding.UTF8.GetString(e.Body));
        }
    }
}