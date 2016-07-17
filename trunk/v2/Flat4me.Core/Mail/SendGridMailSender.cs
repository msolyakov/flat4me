using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mime;

namespace Flat4me.Core.Mail
{
    /// <summary>
    /// Класс, реализующий функциональность доставки сообщений через сервис SendGrid.
    /// </summary>
    /// <remarks>
    /// https://github.com/sendgrid/sendgrid-csharp#how-to-create-an-email
    /// </remarks>
    public class SendGridMailSender : IMailSender
    {
        private string _defaultFromAddress;
        private string _apiKey;
        private SendGridAPIClient _client;

        public SendGridMailSender()
        {
            _defaultFromAddress = Settings.Default.Mail_SendGrid_DefaultFrom;
            _apiKey = Settings.Default.Mail_SendGrid_ApiKey;
        }

        public void SendPlainText(IEnumerable<string> recipients, string subject, string msgbody)
        {
            SendPlainText(String.Empty, recipients, subject, msgbody);
        }

        public void SendPlainText(string from, IEnumerable<string> recipients, string subject, string msgbody)
        {
            string errorMessage;
            if (!this.sendMail(recipients, from, subject, MediaTypeNames.Text.Plain, msgbody, out errorMessage))
            {
                // TODO: Log messsage
                throw new Exception(String.Format("Mail sending error: {0}", errorMessage));
            }
        }

        public void SendHtmlText(IEnumerable<string> recipients, string subject, string msgbody)
        {
            SendHtmlText(String.Empty, recipients, subject, msgbody);
        }

        public void SendHtmlText(string from, IEnumerable<string> recipients, string subject, string msgbody)
        {
            string errorMessage;
            if (!this.sendMail(recipients, from, subject, MediaTypeNames.Text.Html, msgbody, out errorMessage))
            {
                // TODO: Log messsage
                throw new Exception(String.Format("Mail sending error: {0}", errorMessage));
            }
        }

        /// <summary>
        /// Создает и отпарляет почтовое сообщение
        /// </summary>
        private bool sendMail(IEnumerable<string> sendTo, string sendFrom, string subject, string ctype, string msgbody, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                ensureMailClient();
                SendGrid.Helpers.Mail.Mail mail = new SendGrid.Helpers.Mail.Mail();

                // From, subject
                mail.From = (!String.IsNullOrEmpty(sendFrom)) ? new Email(sendFrom) : new Email(_defaultFromAddress);
                mail.Subject = subject;

                // Add recipients
                foreach (string to in sendTo)
                {
                    var personalization = new Personalization();
                    personalization.AddTo(new Email(to));
                    mail.AddPersonalization(personalization);
                }

                // Add mail body
                mail.AddContent(new Content()
                {
                    Type = ctype,
                    Value = msgbody
                });

                // Senging mail
                _client.client.mail.send.post(requestBody: mail.Get());
            }
            catch (Exception e)
            {
                errorMessage = String.Format("{0} \r\n{1}", e.Message, e.StackTrace);
                _client = null;
                return false;
            }

            return true;
        }

        private void ensureMailClient()
        {
            if (_client == null)
            {
                // Web transport for sending emails
                _client = new SendGridAPIClient(_apiKey);
            }
        }


    }
}
