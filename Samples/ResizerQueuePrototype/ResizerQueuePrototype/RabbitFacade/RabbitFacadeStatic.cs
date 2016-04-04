using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitFacade
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
            if ( _connection != null && !_connection.IsOpen )
                ResetConnection();
            
            // Далее стандарная реализация Singleton
            if( _connection == null )
            {
                lock (_lockObj)
                {
                    if ( _connection == null )
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
                    if( _connection.IsOpen )
                        _connection.Close();
                    _connection.Dispose();
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
                VirtualHost = RabbitClientConfig.Default.DefaultVirtualHost
            };

            int port; // Если порт не указан, или указан с ошибкой, используем стандартное значение
            if (Int32.TryParse(RabbitClientConfig.Default.RabbitPort, out port))
            {
                factory.Port = port;
            }

            return factory.CreateConnection();
        }
    }
}
