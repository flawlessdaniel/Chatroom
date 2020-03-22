using Chatroom.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Chatroom.Helpers
{
    public class ChatBot : IBotHelper
    {
        private readonly IMessageBroker obj;

        public ChatBot(IMessageBroker obj)
        {
            this.obj = obj;
        }

        public bool RequestStockQuotes(string command)
        {
            try
            {
                if (String.IsNullOrEmpty(command))
                    return false;

                if (!command.Contains("/stock="))
                    return false;

                string StockCode = (command.Split('=').Length < 2) ? "" : command.Split('=')[1];
                string url = "https://stooq.com/q/l/?s=" + StockCode.ToLower() + "&f=sd2t2ohlcv&h&e=csv";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string results = sr.ReadToEnd();
                sr.Close();

                string message = " Chatbot Say : " + StockCode + ParseCsv(results);

                return this.obj.send(message);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string ParseCsv(string csvData)
        {
            try
            {
                if (String.IsNullOrEmpty(csvData)) return " quote is unavailable ";

                List<string> data = csvData.Split(',').ToList().Where((x, y) => y > 7).ToList();
                return (data.Count >= 5) ? data.Select(x => " quote is $" + Math.Round(Convert.ToDecimal((x[4] + x[5]) / 2), 2).ToString("N2")  + " per share").FirstOrDefault() : " quote is unavailable ";
            }
            catch (Exception ex)
            {
                return " quote is unavailable ";
            }
        }
    }
}