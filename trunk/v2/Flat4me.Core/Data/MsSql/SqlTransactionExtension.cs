using System.Data.SqlClient;

namespace Flat4me.Core.Data.MsSql
{
    public static class SqlTransactionExtension
    {
        public static void RollbackSafe(this SqlTransaction tran)
        {
            // Connection mith be null if SqlServer already did rollbacking
            if (tran != null && tran.Connection != null)
                tran.Rollback();
        }
    }
}
