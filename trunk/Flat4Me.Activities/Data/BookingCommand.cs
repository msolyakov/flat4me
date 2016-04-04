using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4Me.Activities.Data
{
    public enum BookingCommand
    {
        /// <summary>
        /// Подтверждение условий бронирования клиентом
        /// </summary>
        ConfirmByClient,
        /// <summary>
        /// Подтверждение условий бронирования арендатором
        /// </summary>
        ConfirmByOwner,
        /// <summary>
        /// Отослать еще раз клиенту запрос на подтверждение условий
        /// </summary>
        ResendToClient,
        /// <summary>
        /// Отослать еще раз арендатору запрос на подтверждение условий
        /// </summary>
        ResendToOwner,
        /// <summary>
        /// Отмена заказа клиентом до подтверждения условий
        /// </summary>
        CancelDraft,
        /// <summary>
        /// Отмена заказа клиентом до подтверждения условий
        /// </summary>
        CancelBadTerms,
        /// <summary>
        /// Отклонение заказа арендодателем до подтверждения условий по причине отсутствия мест
        /// </summary>
        RejectDraftNoVacancy,
        /// <summary>
        /// Отклонение заказа арендодателем до подтверждения условий по причине спама со стороны клинета
        /// </summary>
        RejectDraftAbuse,
        /// <summary>
        /// Отклонение заказа арендодателем - техническая причина
        /// </summary>
        RejectTechnicalReason,
        /// <summary>
        /// Отклонение заказа арендодателем - плохой клиент
        /// </summary>
        RejectBadClient,
        /// <summary>
        /// Отмена заказа клинетом по личным обстоятельствам
        /// </summary>
        CancelClientReason,
        /// <summary>
        /// Отмена заказа клинетом по причине плохих условий
        /// </summary>
        CancelBadFlat,
        /// <summary>
        /// Клиент заехал в квартиру - нравятся условия
        /// </summary>
        CheckInLike,
        /// <summary>
        /// Клиент заехал в квартиру - не нравятся условия
        /// </summary>
        CheckInNotLike
    }
}
