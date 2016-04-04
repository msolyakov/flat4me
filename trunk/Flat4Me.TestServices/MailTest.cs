using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flat4Me.Core.Rabbit;
using System.Collections.Generic;
using System.Text;
using System.Net.Mime;
using System.Threading.Tasks;
using Flat4Me.Core.Mail;
using System.Threading;
using System.IO;
using Flat4Me.MailServiceCore;

namespace Flat4Me.Tests
{
    [TestClass]
    public class MailTest
    {
        [TestMethod]
        public void MailTest_SendToQueue()
        {
            string subj = String.Format("{0} - Welcome to Flat4.me!", DateTime.Now.ToShortDateString());
            string msg = @"Alex, Michael, hi again! 

It really great to say ""Hooray!"" to you in this mail again. :)
It was sent via SendGrid service API.

I'm sure that it won't be the last message of couse.
--
Yours, 
Mail Test via SendGrid.";

            MailHelper.SendPlainText(
                new string[] { "msolyakov@yandex.ru", "imaleksandr@outlook.com" },
                subj, msg);
        }

        [TestMethod]
        public void MailTest_SendOut()
        {
            bool processMails = true;
            Task processorTask = Task.Run(() =>
            {
                MailQueueProcessor mqp = new MailQueueProcessor(RabbitFacadeConsts.QUEUE_EMAIL_SEND);
                
                // Подписываемся на события ошибок и предупреждений
                mqp.ProcessingError += onError;
                mqp.ProcessingWarning += onError;
                
                // Запускаем обработку
                mqp.DoProcessMessages(ref processMails);
            });
            Thread.Sleep(6000);
            
            processMails = false;
            processorTask.Wait();
        }

        private void onError(object o, ErrorEventArgs ea)
        {
            throw ea.GetException();
        }
    }
}
