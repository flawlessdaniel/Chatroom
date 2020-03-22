using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Unity;
using Unity.Injection;

namespace Chatroom.Helpers
{
    public class DependencyInjector
    {
        private static IUnityContainer UnityContainer;

        public static IUnityContainer ConfigureDependencies()
        {
            ConnectionFactory factory = new ConnectionFactory();
            
            factory.HostName = ConfigurationManager.AppSettings["RabbitMQHost"];
            factory.UserName = ConfigurationManager.AppSettings["RabbitMQUser"];
            factory.Password = ConfigurationManager.AppSettings["RabbitMQPass"];
            factory.Port = Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMQPort"]);

            UnityContainer = new UnityContainer();
            UnityContainer.RegisterType<IDbHelper, SqlHelper>();
            UnityContainer.RegisterType<IMessageBroker, RabbitMQBroker>(new InjectionConstructor(ConfigurationManager.AppSettings["RabbitMQQueu"], factory));
            UnityContainer.RegisterType<IBotHelper, ChatBot>();

            return UnityContainer;
        }

        public static T Retrieve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
    }
}