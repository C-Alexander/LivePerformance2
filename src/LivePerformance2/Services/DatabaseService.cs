using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace LivePerformance2.Services
{
    public class DatabaseService : IDatabaseService
    {
        private DbConnection _connection = new SqlConnection();

        public DbConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

         void Open()
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    _connection.ConnectionString =
                        @"Server=mssql.fhict.local;Database=dbi349135;User Id=dbi349135;Password=jemoederis12"; //doing this proper with DI is very complicated for unit tests. Only change this once I make a memorycontext.
                    //change for security reasons!!!
                    _connection.Open();
                }
                else
                {
                    Log.Information("Tried to open connection, state was: " + _connection.State);
                    Close();
                    Open();
                }
            }
            catch (SqlException e )
            {
                Log.Information(e.StackTrace); // honestly, it should use the logger and probably tbh it shouldnt handle this to begin with - asp.net will hide and fail to display the page on an error by default.
                //thats how it should be. God knows the ungodly effects of a broken db connection on a dynamic site....
            }
        }

        public void RunCommandNonQuery(DbCommand command)
        {
            Open();
            command.Connection = _connection;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Log.Information(e.StackTrace);
                
            }
            Close();
        }

        public object RunScalar(DbCommand command)
        {
            Open();
            command.Connection = _connection;
            try
            {
                object result =  command.ExecuteScalar();
                Close();
                return result;
            }
            catch (SqlException e)
            {
                Log.Information(e.StackTrace);
                return null;

            }
        }

        public DbDataReader RunCommand(DbCommand command)
        {
            Open();
                command.Connection = _connection;
                DbDataReader reader = command.ExecuteReader();
            return reader;
        }

        void Close()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
            else throw new Exception("Tried to close a connection that was not open");
        }
    }
}
