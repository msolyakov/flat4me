using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flat4Me.Data.Repository.Interfaces
{
    public interface ILogRepository
    {
        // Exceptions
        Task LogException(Exception exception);
        Task LogException(Exception exception, CancellationToken cancellationToken);
        Task LogException(string message, Exception exception);
        Task LogException(string message, Exception exception, CancellationToken cancellationToken);

        // Warnings
        Task LogWarning(string message);
        Task LogWarning(string message, Exception reason);

        // Information
        Task LogInfo(string message);
    }
}
