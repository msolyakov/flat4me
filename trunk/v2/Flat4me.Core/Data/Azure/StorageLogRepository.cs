// Logs has to be moved to SQL server using NLog

//using System;
//using System.Linq;
//using System.Text;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.WindowsAzure.Storage;
//using Microsoft.WindowsAzure.Storage.Auth;
//using Microsoft.WindowsAzure.Storage.Table;
//using Flat4me.Core.Data.Objects;
//using Flat4me.Core.Data;

//namespace Flat4me.Core.Data.Azure
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <remarks>
//    /// http://azure.microsoft.com/ru-ru/documentation/articles/storage-dotnet-how-to-use-tables/
//    /// </remarks>
//    public class StorageLogRepository : BaseRepository, ILogRepository
//    {
//        private class StorageLogEntry : TableEntity
//        {
//            public DateTime EventDate { get; set; }
//            public string Message { get; set; }
//            public string Details { get; set; }
//        }

//        private const string ENTRY_LEVEL_ERROR = "Error";
//        private const string ENTRY_LEVEL_WARNING = "Warning";
//        private const string ENTRY_LEVEL_INFORMATION = "Information";

//        private CloudTableClient _tableClient;
//        private CloudTable _table;

//        private string _logTable;

//        public StorageLogRepository()
//        {
//            _logTable = StorageSettings.Default.LogTableName;
//        }

//        public void Init()
//        {
//            ensureTableClient();
//            ensureLogTable();
//        }

//        #region Exceptions

//        // Exceptions
//        public Task LogException(Exception e)
//        {
//            return LogException(String.Empty, e);
//        }
//        /// <summary>
//        /// Throws NotImplementedException.
//        /// </summary>
//        public Task LogException(Exception e, CancellationToken cancellationToken)
//        {
//            return LogException(String.Empty, e, Task.Factory.CancellationToken);
//        }

//        public Task LogException(string message, Exception e)
//        {
//            return LogException(message, e, Task.Factory.CancellationToken);
//        }
//        /// <summary>
//        /// Throws NotImplementedException.
//        /// </summary>
//        public Task LogException(string message, Exception e, CancellationToken cancellationToken)
//        {
//            return Task.Run(() =>
//            {
//                StorageLogEntry logEntry = createLogEntry(ENTRY_LEVEL_ERROR);
//                logEntry.Message = (!String.IsNullOrEmpty(message)) ? message : "N/A";

//                if (e != null)
//                {
//                    logEntry.Details = String.Format("{0}: {1} at \r\n{2}", e.GetType().FullName, e.Message, e.StackTrace);
//                }

//                this.insertLogEntry(logEntry);
//            }, cancellationToken);
//        }

//        #endregion

//        #region Warnings

//        // Warnings
//        public Task LogWarning(string message)
//        {
//            return LogWarning(message, null);
//        }

//        public Task LogWarning(string message, Exception reason)
//        {
//            return Task.Run(() =>
//            {
//                StorageLogEntry logEntry = createLogEntry(ENTRY_LEVEL_WARNING);
//                logEntry.Message = (!String.IsNullOrEmpty(message)) ? message : "N/A";

//                if (reason != null)
//                {
//                    logEntry.Details = String.Format("{0}: {1} at \r\n{2}", reason.GetType().FullName, reason.Message, reason.StackTrace);
//                }

//                this.insertLogEntry(logEntry);
//            });
//        }

//        #endregion

//        #region Info

//        // Information
//        public Task LogInfo(string message)
//        {
//            return Task.Run(() =>
//            {
//                if (String.IsNullOrEmpty(message))
//                    return;

//                StorageLogEntry logEntry = createLogEntry(ENTRY_LEVEL_INFORMATION);
//                logEntry.Message = (!String.IsNullOrEmpty(message)) ? message : "N/A";
//                this.insertLogEntry(logEntry);
//            });
//        }

//        #endregion

//        #region Work with Table Client

//        private StorageLogEntry createLogEntry(string entryLevel)
//        {
//            // Use ticks for native sorting. At top storage will return last records.
//            // Use guid for unique.
//            var rowKey = string.Format("{0}-{1}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks, Guid.NewGuid());

//            return new StorageLogEntry()
//            {
//                PartitionKey = entryLevel,
//                RowKey = rowKey,
//                EventDate = base.DateTimeNow
//            };
//        }

//        private void insertLogEntry(StorageLogEntry logEntry)
//        {
//            try
//            {
//                ensureLogTable();
//                TableOperation insert = TableOperation.Insert(logEntry);
//                _table.Execute(insert);
//            }
//            catch (StorageException e)
//            {
//                _table = null;
//                _tableClient = null;
//            }
//        }

//        private void ensureTableClient()
//        {
//            if (_tableClient != null)
//                return;

//            CloudStorageAccount storageAccount;
//            if (!CloudStorageAccount.TryParse(StorageSettings.Default.ConnectionString, out storageAccount))
//            {
//                throw new Exception("Azure Storage connection string is invalid");
//            }

//            _tableClient = storageAccount.CreateCloudTableClient();
//        }

//        private void ensureLogTable()
//        {
//            ensureTableClient();
//            if (_table != null)
//                return;

//            CloudTable t = _tableClient.GetTableReference(_logTable);
//            t.CreateIfNotExists();

//            _table = t;
//        }

//        #endregion
//    }
//}