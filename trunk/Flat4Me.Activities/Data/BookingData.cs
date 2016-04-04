using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Data
{
    public class BookingData
    {
        // Data ID's
        public int ReservationId { get; set; }
        public int FlatId { get; set; }
        public int OwnerId { get; set; }
        public int ClientId { get; set; }

        // Actions. 
        // Сделано, чтобы использовать короткие URL ~вида do.flat4.me/{id}, с возможностью стухания ссылки и без возможности повторной отправки
        // TODO: Изучить возможность формировать ссылки непосредственно из Workflow
        public string UrlConfirmByOwner { get; set; }
        public string UrlConfirmByClient { get; set; }
        public string UrlCancelDraft { get; set; }
        public string UrlCancelBadTerms { get; set; }
        public string UrlRejectDraftNoVacancy { get; set; }
        public string UrlRejectDraftAbuse { get; set; }
        public string UrlRejectTechnicalReason { get; set; }
        public string UrlRejectBadClient { get; set; }
        public string UrlCancelClientReason { get; set; }
        public string UrlCancelBadFlat { get; set; }
        public string UrlCheckInLike { get; set; }
        public string UrlCheckInNotLike { get; set; }
    }
}
