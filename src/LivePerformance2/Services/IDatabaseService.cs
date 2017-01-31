using System.Data.Common;
using System.Data.SqlClient;

namespace LivePerformance2.Services
{
    public interface IDatabaseService
    {
        DbConnection Connection { get; set; }

        DbDataReader RunCommand(DbCommand command);
        void RunCommandNonQuery(DbCommand command);
        object RunScalar(DbCommand cmd);
    }
}