using Flat4Me.Activities.Data;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Messages.Adapters
{
    public abstract class MailAdapter : AsyncCodeActivity
    {
        [RequiredArgument]
        public InArgument<object> Source { get; set; }

        [RequiredArgument]
        public InArgument<MailData> MailData { get; set; }
        
        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            object source = Source.Get(context);
            MailData mailData = MailData.Get(context);
            
            if(mailData == null)
                throw new ArgumentNullException("MailData is null");

            if (!IsSourceValid(source))
                throw new InvalidCastException("Order Data is invalid");

            return ((Action<AsyncCodeActivityContext, object, MailData>)Fill).BeginInvoke(context, source, mailData, callback, state);
        }

        protected override void EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            // Nothing to do here since there are no OutArgument properties
        }

        /// <summary>
        /// Заполняет объект MailData значениями на основании данных из source
        /// </summary>
        /// <param name="mailData"></param>
        protected abstract void Fill(AsyncCodeActivityContext context, object source, MailData mailData);

        /// <summary>
        /// Проверяет тип и валидность объекта данных, переданного в аргументе
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsSourceValid(object source);
    }
}
