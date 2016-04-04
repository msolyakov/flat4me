using Flat4Me.Activities.Data;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Messages
{
    public sealed class SendSms : CodeActivity
    {
        [RequiredArgument]
        public InArgument<SmsData> Data { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // TODO:
            throw new NotImplementedException();
            //Task.Run(() => 
            //{

            //});
        }
    }
}
