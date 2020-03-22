using System;
using Chatroom;
using Chatroom.Controllers;
using Chatroom.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;

namespace ChatroomTest
{
    [TestClass]
    public class ChatroomTests
    {
        IUnityContainer container;
        public ChatroomTests()
        {
            container = DependencyInjector.ConfigureDependencies();
        }

        [TestMethod]
        public void Empty_Command_Return_False()
        {
            IBotHelper bot = container.Resolve<IBotHelper>();
            Assert.IsFalse(bot.RequestStockQuotes(String.Empty));
            Assert.IsFalse(bot.RequestStockQuotes(null));
            Assert.IsFalse(bot.RequestStockQuotes(""));
        }
        
        [TestMethod]
        public void Bad_StockCode_Command_Return_False()
        {
            IBotHelper bot = container.Resolve<IBotHelper>();
            Assert.IsFalse(bot.RequestStockQuotes("test"));
            Assert.IsFalse(bot.RequestStockQuotes("/stock"));
            Assert.IsFalse(bot.RequestStockQuotes("stock="));
            Assert.IsFalse(bot.RequestStockQuotes("/="));
            Assert.IsFalse(bot.RequestStockQuotes("stock="));
        }

        [TestMethod]
        public void Unrecognized_StockCode_Command_Return_Unavailable()
        {
            IBotHelper bot = container.Resolve<IBotHelper>();
            IMessageBroker rabbit = container.Resolve<IMessageBroker>();
            bot.RequestStockQuotes("/stock=test");
            Assert.AreEqual("Chatbot Say : test quote is unavailable", rabbit.receive().Trim());
        }
    }
}
