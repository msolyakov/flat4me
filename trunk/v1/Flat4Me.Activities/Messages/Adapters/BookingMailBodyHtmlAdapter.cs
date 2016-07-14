using Flat4Me.Activities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Messages.Adapters
{
    public class BookingMailBodyHtmlAdapter : TemplateMailAdapter
    {
        /// <summary>
        /// Проверяет тип и валидность объекта данных, переданного в аргументе
        /// </summary>
        /// <returns></returns>
        protected override bool IsSourceValid(object orderData)
        {
            return (orderData is BookingData);
        }

        /// <summary>
        /// Запрашивает из БД email адреc клиента и передает его в объект MailData
        /// </summary>
        /// <param name="mailData"></param>
        protected override void Fill(object source, MailData mailData, string templateKey)
        {
            BookingData data = (BookingData)source;
            mailData.IsBodyHtml = true;

            // TODO: загрузка и заполнение шаблона
            throw new NotImplementedException();

        }    
    }
}
