using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flat4Me.Activities.Data;

namespace Flat4Me.Activities.Messages.Adapters
{
    /// <summary>
    /// Запрашивает из БД email адреc клиента и передает его в объект MailData
    /// </summary>
    public class BookingMailAddressClientAdapter : BookingMailAdapter
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
