using Flat4Me.Activities.Data;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Messages.Adapters
{
    /// <summary>
    /// Базовый класс адаптеров, использующих шаблоны почтовых сообщений для заполнения объекта MailData
    /// </summary>
    public abstract class TemplateMailAdapter : MailAdapter
    {
        [RequiredArgument]
        public InArgument<string> TemplateKey { get; set; }

        protected override void Fill(AsyncCodeActivityContext context, object source, MailData mailData)
        {
            string templateKey = TemplateKey.Get(context);

            if (String.IsNullOrEmpty(templateKey))
                throw new ArgumentNullException("TemplateKey is null");

            Fill(source, mailData, templateKey);
        }
        
        /// <summary>
        /// Заполняет объект MailData значениями на основании данных из source и templateKey
        /// </summary>
        protected abstract void Fill(object source, MailData mailData, string templateKey);
    }
}
