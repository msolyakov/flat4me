using Flat4Me.Activities.Data;
using Flat4Me.Core.Mail;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Messages
{
    public sealed class SendMail : CodeActivity
    {
        [RequiredArgument]
        public InArgument<MailData> Data { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Task.Run(() => 
            {
                MailData data = Data.Get(context);
                if (data == null)
                    return;
                
                if (data.IsBodyHtml)
                {
                    MailHelper.SendHtmlText(data.From, data.To, data.Subject, data.Body);
                }
                else
                {
                    MailHelper.SendPlainText(data.From, data.To, data.Subject, data.Body);
                }
            });
        }
    }
}
