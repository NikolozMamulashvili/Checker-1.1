using System;
using System.Data;
using System.Data.SqlClient;

namespace WebsiteSafetyChecker
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void InitializeDatabase()
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WebsiteSafety')
                        BEGIN
                            CREATE TABLE WebsiteSafety (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Url NVARCHAR(500) NOT NULL,
                                IsSafe BIT NOT NULL
                            )
                        END";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
} 
