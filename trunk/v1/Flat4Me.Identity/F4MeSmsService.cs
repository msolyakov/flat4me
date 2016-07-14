using Flat4Me.Core.Mail;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace Flat4Me.Identity
{
    public class F4MeSmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.Factory.StartNew(() =>
            {
                var smsc = new SMSC();
                var r = smsc.send_sms(message.Destination, message.Body);
                
                //var accountSid = "ACc8186043069be693eca071087897f5e7";                
                //var authToken = "a823b6f9a6fdb3334927feb983311fd9";
                //var twilioPhoneNumber = "+4915735982029";

                //var twilio = new TwilioRestClient(accountSid, authToken);

                //var r = twilio.SendSmsMessage(twilioPhoneNumber, message.Destination, message.Body);
                //var b = r.Status;
            });
        }
    }
}
