using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom.Helpers
{
    public interface IMessageBroker
    {
        
        bool send(string message);

        string receive();

        bool suscribe();
    }
}
