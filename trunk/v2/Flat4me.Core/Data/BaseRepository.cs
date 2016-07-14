using System;
using System.Configuration;

namespace Flat4me.Core.Data
{
    public abstract class BaseRepository
    {
        private string _connectionString = null;
        protected readonly int DbAppId = 1;

        protected DateTime DateTimeNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Переопределяемое значение строки подключения.
        /// Если значение не задано, то пытаемся получить строку подключения из конфига.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (String.IsNullOrEmpty(_connectionString))
                { 
                    // Инициализируем строку подключения из конфигурации.
                    try
                    {
                        _connectionString = ConfigurationManager.ConnectionStrings["F4MeDev"].ConnectionString;
                    }
                    catch
                    {
                        _connectionString = null;
                    }
                }

                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }
    }
}
