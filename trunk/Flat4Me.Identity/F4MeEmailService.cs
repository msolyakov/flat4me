using Flat4Me.Core.Mail;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Identity
{
    public class F4MeEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.Factory.StartNew(() =>
            {
                var recipients = message.Destination.Split(';');

                MailHelper.SendHtmlText(recipients, "Квартира для меня: " + message.Subject, message.Body);
            });
        }
    }
}
