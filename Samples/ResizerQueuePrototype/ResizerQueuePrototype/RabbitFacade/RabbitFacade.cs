using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Framing;
using System.Net.Mime;
using RabbitMQ.Client.Events;

namespace RabbitFacade
{
    /// <summary>
    /// Класс для работы с одной выбранной очередью сообщений.
    /// Инициализирует внутри себя экземпляр IModel.
    /// Реализует IDisposable. Рассчитан на работу внутри using.
    /// Mikhail Solyakov.
    /// </summary>
    public class RabbitFacade : IDisposable
    {
        private IModel _channel;
        private QueueingBasicConsumer _consumer;

        private readonly string _exchange = String.Empty; 
        private string _queueName;
        private bool _durable = true;

        #region Initialize/Dispose

        /// <summary>
        /// Создает новый экземпляр RabbitFacade. 
        /// Инициализирует экземпляр IModel для дальнейшей работы.
        /// </summary>
        public RabbitFacade(string queueName, bool durable = true)
        {
            if (String.IsNullOrEmpty(queueName))
                throw new Exception("Unable to create RabbitFacade instance. QueueName is null.");
            
            var conn = RabbitFacadeStatic.GetConnection();
            if (conn == null)
                throw new ConnectFailureException("Unable to create RabbitFacade instance. Connection is null.", null);

            _channel = conn.CreateModel();
            _queueName = queueName;
            _durable = durable;

            EnsureQueue();
        }

        /// <summary>
        /// Закрывает текущий IModel, если тот еще открыт.
        /// </summary>
        public void Dispose()
        {
            if (_channel != null)
            {
                if( _channel.IsOpen ) 
                    _channel.Close();
                _channel.Dispose();
            }
        }

        /// <summary>
        /// Проверяет наличие очереди и создает ее при отсутствии.
        /// </summary>
        public void EnsureQueue()
        {
            _channel.QueueDeclare(_queueName, _durable, false, false, null);
        }

        public void DeleteQueue(string queueName)
        {
            _channel.QueueDelete(queueName);
        }

        #endregion

        #region Transactions

        public void BeginTran()
        {
            _channel.TxSelect();
        }

        public void Commit()
        {
            _channel.TxCommit();
        }

        public void Rollback()
        {
            _channel.TxRollback();
        }

        #endregion

        #region Publish

        /// <summary>
        /// Overloaded. Посылает сообщение в текущую очередь.
        /// </summary>
        /// <param name="contentType">Строка, определяющая тип содержимого сообщения</param>
        /// <param name="messageBody">Массив байт с данными сообщения</param>
        public void Publish(string contentType, byte[] messageBody)
        {
            ContentType ctype = new ContentType(contentType);
            Publish(ctype, messageBody);
        }
        
        /// <summary>
        /// Overloaded. Посылает сообщение в текущую очередь.
        /// </summary>
        /// <param name="contentType">Определяет тип содержимого сообщения</param>
        /// <param name="messageBody">Массив байт с данными сообщения</param>
        public void Publish(ContentType contentType, byte[] messageBody)
        {
            if (contentType == null)
                throw new Exception(String.Format("Unable to publish message in \"{0}\". ContentType is null.", _queueName));

            if (messageBody == null || messageBody.LongLength == 0)
                throw new Exception(String.Format("Unable to publish message in \"{0}\". Message body is empty.", _queueName));
            
            IBasicProperties props = new BasicProperties();
            props.ContentType = contentType.ToString(); 
            props.SetPersistent(_durable);
            
            _channel.BasicPublish(_exchange, _queueName, props, messageBody);
        }

        #endregion

        #region Consume

        /// <summary>
        /// Инициализирует получателя сообщений.
        /// </summary>
        /// <returns>Объект типа QueueingBasicConsumer</returns>
        public QueueingBasicConsumer InitConsumer()
        { 
            _consumer = new QueueingBasicConsumer(_channel);
			_channel.BasicConsume(_queueName, false, _consumer);

            return _consumer;
        }

        /// <summary>
        /// Получает следующее сообщение из очереди после истечения заданного таймаута
        /// </summary>
        /// <param name="timeout">Время ожидания следующего сообщения (в миллисекундах)</param>
        /// <param name="message">Объект Tuple, содержащий ContentType сообщения, тело сообщения (byte[]), 
        /// признак повторной доставки (bool), и DeliveryTag для последующего уведомления (ulong)</param>
        /// <returns>Признак наличия сообщения</returns>
        public bool Next(int timeout, out Tuple<ContentType, byte[], bool, ulong> message)
        {
            message = null;

            BasicDeliverEventArgs ea;
            if (_consumer != null && _consumer.Queue.Dequeue(timeout, out ea))
            {
                ContentType ctype = (!String.IsNullOrEmpty(ea.BasicProperties.ContentType)) 
                    ? new ContentType(ea.BasicProperties.ContentType) : null;

                message = new Tuple<ContentType, byte[], bool, ulong>(ctype,
                    ea.Body, ea.Redelivered, ea.DeliveryTag);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Для указанного DeliveryTag выполняет Ack или Nack (в зависимости от флага Delivered).
        /// </summary>
        /// <param name="deliveryTag">DeliveryTag сообщения</param>
        /// <param name="delivered">флаг Delivered</param>
        public void SetDelivered(ulong deliveryTag, bool delivered)
        {
            if(delivered)
            {
                _channel.BasicAck(deliveryTag, false);
            }
            else
            {
                _channel.BasicNack(deliveryTag, false, true);
            }
        }

        #endregion
    }
}
