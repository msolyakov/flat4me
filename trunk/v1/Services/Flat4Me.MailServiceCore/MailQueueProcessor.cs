using Flat4Me.Core.Rabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using System.Net.Mail;
using System.Net;
using Flat4Me.Core.Mail;

namespace Flat4Me.MailServiceCore
{
    /// <summary>
    /// Класс, реализующий функциональность получения 
    /// email-сообщений из очереди и их доставки через сервис SendGrid.
    /// </summary>
    /// <remarks>
    /// https://github.com/sendgrid/sendgrid-csharp#how-to-create-an-email
    /// http://azure.microsoft.com/en-us/documentation/articles/sendgrid-dotnet-how-to-send-email/
    /// </remarks>
    public class MailQueueProcessor : QueueProcessorBase
    {
        private string _defaultFromAddress;
        private NetworkCredential _credentials;
        private SendGrid.Web _transport;

        /// <summary>
        /// Создает MailQueueProcessor, инициализированный 
        /// именем очереди RabbitFacadeConsts.QUEUE_EMAIL_SEND
        /// </summary>
        public MailQueueProcessor()
            : this(RabbitFacadeConsts.QUEUE_EMAIL_SEND)
        { 
        }
        
        public MailQueueProcessor(string queueName)
            : base(queueName)
        {
            _defaultFromAddress = MailServiceSettings.Default.DefaultFromAddress;
            _credentials = new NetworkCredential(MailServiceSettings.Default.SendGridUser, MailServiceSettings.Default.SendGridPassword);
        }

        /// <summary>
        /// Отправляет письмо получателям, указанным в Headers, через сервис SendGrid.
        /// </summary>
        /// <returns>True, если сообщение успешно отправлено</returns>
        protected override bool ProcessMessage(ContentType ctype, IDictionary<string, object> headers, byte[] msgbody, bool redelivered, out string errorMessage)
        {
            errorMessage = null;

            // Проверям ContentType и заголовки
            if (ctype == null || headers == null)
            {
                // Не задан ContentType/Заголовки. Игнорируем сообщение.
                errorMessage = "Content Type and Headers must be specified for Mail message.";
                return false;
            }

            // Проверям ContentType
            if (ctype.MediaType != MediaTypeNames.Text.Plain && ctype.MediaType != MediaTypeNames.Text.Html)
            {
                // Неверный ContentType. Игнорируем сообщение.
                errorMessage = String.Format("Invalid Content Type for Mail message: {0}. Html or Plain text expected.", ctype.MediaType);
                return false;
            }

            if (String.IsNullOrEmpty(ctype.CharSet))
            {
                // Неверный ContentType. Игнорируем сообщение.
                errorMessage = "Character Set wasn't specified for Mail message.";
                return false;
            }

            string sendTo;
            string sendFrom;
            string subject;

            // Загружаем заголовки
            MailHelper.ParseHeaders(headers, out sendFrom, out sendTo, out subject);
            
            if (String.IsNullOrEmpty(sendTo))
            {
                // Не задан адрес отсылки. Игнорируем сообщение.
                errorMessage = "Send Address is empty.";
                return false;
            }

            // Отсылаем письмо
            return sendMail(sendTo, sendFrom, subject, ctype, msgbody, out errorMessage);
        }

        /// <summary>
        /// Создает и отпарляет почтовое сообщение
        /// </summary>
        private bool sendMail(string sendTo, string sendFrom, string subject, ContentType ctype, byte[] msgbody, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                ensureMailTransport();
                SendGridMessage mail = new SendGridMessage();

                mail.From = (!String.IsNullOrEmpty(sendFrom)) ? new MailAddress(sendFrom) : new MailAddress(_defaultFromAddress);
                mail.AddTo(MailHelper.SplitAddresses(sendTo));
                mail.Subject = subject;

                // Получаем строку тела сообщения
                string body = Encoding.GetEncoding(ctype.CharSet).GetString(msgbody);
                // Сохраняем тело сообщения, в зависимости от значения ContentType
                if (ctype.MediaType == MediaTypeNames.Text.Html)
                {
                    mail.Html = body;
                }
                else
                {
                    mail.Text = body;
                }

                _transport.Deliver(mail);
            }
            catch(Exception e)
            {
                errorMessage = String.Format("{0} \r\n{1}", e.Message, e.StackTrace);
                _transport = null;
                return false;
            }

            return true;
        }

        private void ensureMailTransport()
        {
            if( _transport == null )
            {
                // Web transport for sending emails
                _transport = new Web(_credentials);
            }
        }
    }
}
