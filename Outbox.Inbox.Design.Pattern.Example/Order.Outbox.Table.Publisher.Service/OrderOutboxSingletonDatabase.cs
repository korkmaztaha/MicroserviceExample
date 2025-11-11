using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Order.Outbox.Table.Publisher.Service
{
    public static class OrderOutboxSingletonDatabase
    {
        private static IDbConnection? _connection;
        private static bool _dataReaderState = true;

        public static void Initialize(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MSSQLServer");
            _connection = new SqlConnection(connectionString);
        }

        public static IDbConnection Connection
        {
            get
            {
                if (_connection == null)
                    throw new InvalidOperationException("Database not initialized. Call Initialize() first.");

                if (_connection.State == ConnectionState.Closed)
                    _connection.Open();

                return _connection;
            }
        }

        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql)
            => await Connection.QueryAsync<T>(sql);

        public static async Task<int> ExecuteAsync(string sql)
            => await Connection.ExecuteAsync(sql);

        public static void DataReaderReady() => _dataReaderState = true;
        public static void DataReaderBusy() => _dataReaderState = false;
        public static bool DataReaderState => _dataReaderState;
    }
}
