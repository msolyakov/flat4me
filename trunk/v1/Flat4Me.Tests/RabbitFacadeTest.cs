using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flat4Me.Core.Rabbit;
using System.Text;
using System.Net.Mime;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flat4Me.Tests
{
    [TestClass]
    public class RabbitFacadeTest
    {
        public const string TEST_QUEUE = "TestQueue";
        
        [TestMethod]
        public void Rabbit_Pubish()
        {
            using(RabbitFacade rf = new RabbitFacade(TEST_QUEUE))
            {
                rf.BeginTran();
                string message = "Hello, Universe!";
                rf.Publish("text/html", null, Encoding.UTF8.GetBytes(message));
                rf.Commit();
            }
        }

        [TestMethod]
        public void Rabbit_Consume()
        {
            using (RabbitFacade rf = new RabbitFacade(TEST_QUEUE))
            {
                rf.InitConsumer();

                Tuple<ContentType, IDictionary<string, object>, byte[], bool, ulong> message;
                while (rf.Next(2000, out message))
                {
                    ContentType ctype = message.Item1;
                    byte[] messageBody = message.Item3;
                    Debug.WriteLine(String.Format("Message delivered. Content type: {0}. Bytes: {1}. Value: {2}", 
                        ctype.MediaType, messageBody.LongLength, Encoding.UTF8.GetString(messageBody)));
                    rf.SetDelivered(message.Item5, true);
                }
            }
        }
    }
}
