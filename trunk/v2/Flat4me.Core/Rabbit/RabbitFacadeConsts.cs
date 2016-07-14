using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flat4me.Core.Rabbit
{
    /// <summary>
    /// Основные константы для работы с RabbitMQ
    /// </summary>
    public static class RabbitFacadeConsts
    {
        // Обменники
        public static string EXCHANGE_DEFAULT = String.Empty;

        // Очереди - up to 255 bytes of UTF-8 characters
        public static string QUEUE_IMAGE_MAIN = "f4me.Queue.Image.Main";
        public static string QUEUE_IMAGE_USER = "f4me.Queue.Image.User";
        public static string QUEUE_EMAIL_SEND = "f4me.Queue.Email.Send";
        public static string QUEUE_SMS_SEND =   "f4me.Queue.SMS.Send";

        // Общие константы ключей в Message Headers
        public static string HEADER_USER_ID = "UserID";
        public static string HEADER_FLAT_ID = "FlatID";
        public static string HEADER_IMAGE_ID = "ImageID";
        public static string HEADER_EMAIL_SEND_TO = "EmailSendTo"; 
        public static string HEADER_EMAIL_SEND_FROM = "EmailSendFrom";
        public static string HEADER_EMAIL_SUBJECT = "EmailSubject";
        public static string HEADER_SMS_SEND_TO = "SmsSendTo";
    }
}
