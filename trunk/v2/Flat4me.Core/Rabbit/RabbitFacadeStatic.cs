using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.IO;

namespace Flat4me.Core.Rabbit
{
    /// <summary>
    /// Класс управления подключением к RabbitMQ
    /// </summary>
    public static class RabbitFacadeStatic
    {
        private static IConnection _connection;
        private static readonly object _lockObj = new Object();

        /// <summary>
        /// Returns IConnection object for RabbitMQ client
        /// </summary>
        /// <returns>IConnection instance</returns>
        public static IConnection GetConnection()
        {
            // Удаляем закрытое соедиение
            if (_connection != null && !_connection.IsOpen)
                ResetConnection();

            // Далее стандарная реализация Singleton
            if (_connection == null)
            {
                lock (_lockObj)
                {
                    if (_connection == null)
                    {
                        _connection = CreateConnection();
                    }
                }
            }

            return _connection;
        }

        /// <summary>
        /// Вызвается при завершении работы или неизвестных ошибках IConnection.
        /// </summary>
        public static void ResetConnection()
        {
            lock (_lockObj)
            {
                if (_connection != null)
                {
                    if (_connection.IsOpen)
                        _connection.Close();
                    try
                    {
                        _connection.Dispose();
                    }
                    // Эти исключения должны обрабатываться при Сonnection.Dispose()
                    // http://stackoverflow.com/questions/12499174/rabbitmq-c-sharp-driver-stops-receiving-messages
                    catch (OperationInterruptedException) { }
                    catch (EndOfStreamException) { }
                }
                _connection = null;
            }
        }

        /// <summary>
        /// Creates new IConnection object for RabbitMQ client
        /// </summary>
        /// <returns>IConnection instance</returns>
        static IConnection CreateConnection()
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = RabbitClientConfig.Default.RabbitHost,
                UserName = RabbitClientConfig.Default.RabbitUser,
                Password = RabbitClientConfig.Default.RabbitPassword,
                VirtualHost = RabbitClientConfig.Default.DefaultVirtualHost,
                RequestedHeartbeat = 30
            };

            int port; // Если порт не указан, или указан с ошибкой, используем стандартное значение
            if (Int32.TryParse(RabbitClientConfig.Default.RabbitPort, out port))
            {
                factory.Port = port;
            }

            return factory.CreateConnection();
        }

        public static string GetHeaderAsString(IDictionary<string, object> headers, string key)
        {
            string result = String.Empty;
            
            if (headers.Keys.Contains(key))
            {
                object headerObj = headers[key];
                // проверяем на соответвие массиву байт
                if (headerObj is byte[])
                {
                    byte[] bytes = (byte[])headerObj;
                    result = (bytes.LongLength > 0) ? Encoding.UTF8.GetString(bytes) : String.Empty;
                }
                // проверяем на соответвие строке
                else if (headerObj is string)
                {
                    result = (string)headerObj;
                }
                else if (headerObj != null)
                {
                    result = headerObj.ToString();
                }
            }

            return result;
        }
    }
}