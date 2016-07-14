using Flat4Me.Activities.Data;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Actions
{
    public sealed class GetBookingUrl : CodeActivity
    {
        [RequiredArgument]
        public InArgument<int> ReservationId { get; set; }

        [RequiredArgument]
        public InArgument<BookingCommand> Command { get; set; }

        [RequiredArgument]
        public InArgument<int> Livetime { get; set; }
        
        public OutArgument<string> ActionUrl { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            // TODO: Запрос к БД на создание ссылки для письма
            throw new NotImplementedException();
            //Task.Run(() => 
            //{

            //});
        }
    }
}
