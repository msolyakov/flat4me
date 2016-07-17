using Flat4me.Core.Rabbit;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace Flat4me.Core.Mail
{
    public class RabbitMailSender : IMailSender
    {
        public void SendPlainText(IEnumerable<string> recipients, string subject, string msgbody)
        {
            SendPlainText(String.Empty, recipients, subject, msgbody);
        }
        
        public void SendPlainText(string from, IEnumerable<string> recipients, string subject, string msgbody)
        {
            string sendTo = MailHelper.ConcatAddresses(recipients);
            if (String.IsNullOrEmpty(sendTo))
                throw new Exception("Unable to send mail with no recipients");
            
            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add(RabbitFacadeConsts.HEADER_EMAIL_SEND_FROM, (from != null) ? from : String.Empty);
            headers.Add(RabbitFacadeConsts.HEADER_EMAIL_SEND_TO, sendTo);
            headers.Add(RabbitFacadeConsts.HEADER_EMAIL_SUBJECT, subject);

            // Text/Plain. Кодовая страница обязательна для передачи в письме
            ContentType ctype = new ContentType(MediaTypeNames.Text.Plain);
            ctype.CharSet = Encoding.UTF8.HeaderName;

            // Проверяем ctype.CharSet на этапе отправки
            byte[] msgBytes = Encoding.GetEncoding(ctype.CharSet).GetBytes(msgbody);
            // Отсылаем сообщение в очередь для дальнейшей отправки
            using (RabbitFacade rf = new RabbitFacade(RabbitFacadeConsts.QUEUE_EMAIL_SEND))
            {
                rf.BeginTran();
                rf.Publish(ctype, headers, msgBytes);
                rf.Commit();
            }            
        }

        public void SendHtmlText(IEnumerable<string> recipients, string subject, string msgbody)
        {
            SendHtmlText(String.Empty, recipients, subject, msgbody);
        }

        public void SendHtmlText(string from, IEnumerable<string> recipients, string subject, string msgbody)
        {
            string sendTo = MailHelper.ConcatAddresses(recipients);
            if (String.IsNullOrEmpty(sendTo))
                throw new Exception("Unable to send mail with no recipients");

            Dictionary<string, object> headers = new Dictionary<string, object>();
            headers.Add(RabbitFacadeConsts.HEADER_EMAIL_SEND_FROM, (from != null) ? from : String.Empty);
            headers.Add(RabbitFacadeConsts.HEADER_EMAIL_SEND_TO, sendTo);
            headers.Add(RabbitFacadeConsts.HEADER_EMAIL_SUBJECT, subject);

            // Text/Plain. Кодовая страница обязательна для передачи в письме
            ContentType ctype = new ContentType(MediaTypeNames.Text.Html);
            ctype.CharSet = Encoding.UTF8.HeaderName;

            // Проверяем ctype.CharSet на этапе отправки
            byte[] msgBytes = Encoding.GetEncoding(ctype.CharSet).GetBytes(msgbody);
            // Отсылаем сообщение в очередь для дальнейшей отправки
            using (RabbitFacade rf = new RabbitFacade(RabbitFacadeConsts.QUEUE_EMAIL_SEND))
            {
                rf.BeginTran();
                rf.Publish(ctype, headers, msgBytes);
                rf.Commit();
            }
        }

        public static void ParseHeaders(IDictionary<string, object> headers, out string sendFrom, out string sendTo, out string subject)
        {
            sendFrom = RabbitFacadeStatic.GetHeaderAsString(headers, RabbitFacadeConsts.HEADER_EMAIL_SEND_FROM);
            sendTo = RabbitFacadeStatic.GetHeaderAsString(headers, RabbitFacadeConsts.HEADER_EMAIL_SEND_TO);
            subject = RabbitFacadeStatic.GetHeaderAsString(headers, RabbitFacadeConsts.HEADER_EMAIL_SUBJECT);
        }
    }
}
