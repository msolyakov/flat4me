using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flat4Me.Core.Rabbit
{
    public abstract class QueueProcessorBase
    {
        public const int RECONNECTION_MAX_ATTEMPTS = 10;
        public const int RECONNECTION_ATTEMPT_DELAY = 500;

        private string _queueName;

        public QueueProcessorBase(string queueName)
        {
            this._queueName = queueName;
            this.SilentReconnections = true;
        }

        public string Host
        {
            get { return RabbitClientConfig.Default.RabbitHost; }
        }

        public string Port
        {
            get { return RabbitClientConfig.Default.RabbitPort; }
        }

        public string Vhost
        {
            get { return RabbitClientConfig.Default.DefaultVirtualHost; }
        }

        #region Error handling events

        public event ErrorEventHandler ProcessingError;
        public event ErrorEventHandler ProcessingWarning;

        protected void OnProcessingError(Exception e)
        {
            if (ProcessingError != null)
            {
                ProcessingError(this, new ErrorEventArgs(e));
            }
        }

        protected void OnProcessingWarning(string message, Exception e)
        {
            if (ProcessingWarning != null)
            {
                Exception wrn = new Exception(message, e);
                ProcessingWarning(this, new ErrorEventArgs(wrn));
            }
        }

        /// <summary>
        /// Если true, то в случае успешного переподключения к очереди запись в лог не делается.
        /// По умолчанию - true;
        /// </summary>
        public bool SilentReconnections { get; set; }

        #endregion

        #region Processing messages infrastructure

        /// <summary>
        /// Цикл обработки сообщений
        /// </summary>
        public void DoProcessMessages(ref bool processMessages)
        {
            int reconnAttempts = 0;

            try
            {
                // Допускаем три попытки переподключения к очереди в случае ошибки
                while (processMessages && reconnAttempts <= RECONNECTION_MAX_ATTEMPTS)
                {
                    try
                    {
                        using (RabbitFacade rf = new RabbitFacade(_queueName))
                        {
                            rf.InitConsumer();

                            // Цикл обработки сообщений
                            while (processMessages)
                            {
                                Tuple<ContentType, IDictionary<string, object>, byte[], bool, ulong> message;
                                if (!rf.Next(100, out message))
                                {
                                    reconnAttempts = 0;
                                    continue;
                                }

                                string errorMessage;
                                bool saved = ProcessMessage(message.Item1, message.Item2, message.Item3, message.Item4, out errorMessage);
                                if (!saved)
                                {
                                    OnProcessingWarning(errorMessage, null);
                                }

                                // Передаем в очередь признак доставки сообщения
                                rf.SetDelivered(message.Item5, saved);

                                // Если ошибок нет, то сбрасываем счетчик переподключений
                                reconnAttempts = 0;
                            }
                        }
                    }
                    catch (RabbitMQ.Client.Exceptions.ConnectFailureException cf_ex)
                    {
                        if (!this.reconnect(cf_ex, ref reconnAttempts, !SilentReconnections))
                            throw cf_ex;
                    }
                    catch (RabbitMQ.Client.Exceptions.OperationInterruptedException oi_ex)
                    {
                        if (!this.reconnect(oi_ex, ref reconnAttempts, !SilentReconnections))
                            throw oi_ex;
                    }
                    catch (System.IO.EndOfStreamException eos_ex)
                    {
                        if (!this.reconnect(eos_ex, ref reconnAttempts, !SilentReconnections))
                            throw eos_ex;
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnProcessingError(ex);
            }
            finally
            {
                // Закрываем соединение 
                RabbitFacadeStatic.ResetConnection();                
            }
        }

        /// <summary>
        /// Проверяет количество попыток переподключения, и если OK, то передает ошибку подписчику события ProcessingWarning и сбрасывает соединение.
        /// </summary>
        /// <returns>True, если не превышено макс. количество попыток переподключения</returns>
        private bool reconnect(Exception e, ref int reconnAttempts, bool logAttempt)
        {
            reconnAttempts++;

            if (reconnAttempts <= RECONNECTION_MAX_ATTEMPTS)
            {
                if(logAttempt)
                {
                    OnProcessingWarning(
                        String.Format("Connection refused. Reconnection attempt - {0}.", reconnAttempts), e);
                }

                RabbitFacadeStatic.ResetConnection();
                Thread.Sleep(RECONNECTION_ATTEMPT_DELAY);

                return true;
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Абстрактный метод обработки полученного сообщения.
        /// </summary>
        /// <param name="ctype">Тип содержимого</param>
        /// <param name="headers">Заголовки сообщения</param>
        /// <param name="msgbody">Тело сообщения</param>
        /// <param name="redelivered">Признак повторной доставки</param>
        /// <returns></returns>
        protected abstract bool ProcessMessage(ContentType ctype, IDictionary<string, object> headers, byte[] msgbody, bool redelivered, out string errorMessage);
    }
}
