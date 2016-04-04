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
    /// Запрашивает из БД email адреc арендодателя и передает его в поле "To" объекта MailData
    /// </summary>
    public class BookingMailAddressOwnerAdapter : BookingMailAdapter
    {
        /// <summary>
        /// Запрашивает из БД email адреc клиента и передает его в объект MailData
        /// </summary>
        /// <param name="mailData"></param>
        protected override void Fill(BookingData data, MailData mailData)
        {
            // TODO: запрос к базе за адресом 
            throw new NotImplementedException();
        }
    }
}
