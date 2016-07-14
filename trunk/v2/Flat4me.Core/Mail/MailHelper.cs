using Flat4me.Core.Rabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Mail
{
    public class MailHelper
    {
        /// <summary>
        /// Символ разделителя для адресной строки электронной почты
        /// </summary>
        public const string MAIL_ADDRESSES_SEPARATOR = ",";

        /// <summary>
        /// Отсылает plain/text сообщение в кодировке UTF-8.
        /// В качестве отправителя будет указан адрес по умолчанию, заданный в конфиге.
        /// </summary>
        /// <param name="recipients">Список адресатов</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="msgbody">Тело сообщения</param>
        public static void SendPlainText(IEnumerable<string> recipients, string subject, string msgbody)
        {
            SendPlainText(String.Empty, recipients, subject, msgbody);
        }
        
        /// <summary>
        /// Отсылает plain/text сообщение в кодировке UTF-8.
        /// </summary>
        /// <param name="from">Адрес отправителя. Может быть пустым.</param>
        /// <param name="recipients">Список адресатов</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="msgbody">Тело сообщения</param>
        public static void SendPlainText(string from, IEnumerable<string> recipients, string subject, string msgbody)
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


        /// <summary>
        /// Отсылает html сообщение в кодировке UTF-8.
        /// В качестве отправителя будет указан адрес по умолчанию, заданный в конфиге.
        /// </summary>
        /// <param name="recipients">Список адресатов</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="msgbody">Тело сообщения</param>
        public static void SendHtmlText(IEnumerable<string> recipients, string subject, string msgbody)
        {
            SendHtmlText(String.Empty, recipients, subject, msgbody);
        }

        /// <summary>
        /// Отсылает html сообщение в кодировке UTF-8.
        /// </summary>
        /// <param name="from">Адрес отправителя. Может быть пустым.</param>
        /// <param name="recipients">Список адресатов</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="msgbody">Тело сообщения</param>
        public static void SendHtmlText(string from, IEnumerable<string> recipients, string subject, string msgbody)
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
        
        /// <summary>
        /// Разделяет строку адресов на массив строк, используя MAIL_ADDRESSES_SEPARATOR как разделитель.
        /// </summary>
        public static IEnumerable<string> SplitAddresses(string addresses)
        {
            // 1. Нет адресов
            if (String.IsNullOrEmpty(addresses))
            {
                return new string[] { };
            }

            // 2. Строка включает в себя только один адрес
            if (!addresses.Contains(MAIL_ADDRESSES_SEPARATOR))
            {
                return new string[] { addresses.Trim() };
            }

            // 3. Строка включает в себя два и более адресов
            string[] addressesList = addresses.Split(new string[] { MAIL_ADDRESSES_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < addressesList.Length; i++)
            {
                addressesList[i] = addressesList[i].Trim(); 
            }

            return addressesList;
        }

        /// <summary>
        /// Собирает адресную строку из переданного списка, используя MAIL_ADDRESSES_SEPARATOR как разделитель.
        /// </summary>
        public static string ConcatAddresses(IEnumerable<string> addressesList)
        {
            if (addressesList == null)
                return String.Empty;
            
            // Собираем строку
            StringBuilder builder = new StringBuilder();
            IEnumerator<string> e = addressesList.GetEnumerator();
            while (e.MoveNext())
            {
                builder.Append(MAIL_ADDRESSES_SEPARATOR + e.Current.Trim());
            }

            string addresses = builder.ToString();
            if (addresses.Length >= MAIL_ADDRESSES_SEPARATOR.Length)
            {
                // Удаляем первый разделитель
                addresses = addresses.Substring(MAIL_ADDRESSES_SEPARATOR.Length);
            }

            return addresses;
        }

        public static void ParseHeaders(IDictionary<string, object> headers, out string sendFrom, out string sendTo, out string subject)
        {
            sendFrom = RabbitFacadeStatic.GetHeaderAsString(headers, RabbitFacadeConsts.HEADER_EMAIL_SEND_FROM);
            sendTo = RabbitFacadeStatic.GetHeaderAsString(headers, RabbitFacadeConsts.HEADER_EMAIL_SEND_TO);
            subject = RabbitFacadeStatic.GetHeaderAsString(headers, RabbitFacadeConsts.HEADER_EMAIL_SUBJECT);
        }
    }
}
