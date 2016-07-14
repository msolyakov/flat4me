using Flat4Me.Activities.Data;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Messages.Adapters
{
    public abstract class BookingMailAdapter : MailAdapter
    {
        /// <summary>
        /// Проверяет тип и валидность объекта данных, переданного в аргументе
        /// </summary>
        /// <returns></returns>
        protected override bool IsSourceValid(object source)
        {
            return (source is BookingData);
        }

        /// <summary>
        /// Запрашивает из БД email адреc клиента и передает его в объект MailData
        /// </summary>
        /// <param name="mailData"></param>
        protected override void Fill(AsyncCodeActivityContext context, object source, MailData mailData)
        {
            BookingData data = (BookingData)source;
            this.Fill(data, mailData);
        }

        /// <summary>
        /// Заполняет объект MailData значениями на основании данных из BookingData
        /// </summary>
        protected abstract void Fill(BookingData bookingData, MailData mailData);
    }
}
