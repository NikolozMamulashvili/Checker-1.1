using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WebsiteSafetyChecker
{
    public class CRUD
    {
        private readonly DatabaseConnection _dbConnection;

        public CRUD(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public WebsiteSafety GetById(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM WebsiteSafety WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new WebsiteSafety
                            {
                                Id = reader.GetInt32(0),
                                Url = reader.GetString(1),
                                IsSafe = reader.GetBoolean(2),
                                CheckedDate = reader.GetDateTime(3),
                                Description = reader.IsDBNull(4) ? null : reader.GetString(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<WebsiteSafety> GetAll()
        {
            var websites = new List<WebsiteSafety>();
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM WebsiteSafety";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            websites.Add(new WebsiteSafety
                            {
                                Id = reader.GetInt32(0),
                                Url = reader.GetString(1),
                                IsSafe = reader.GetBoolean(2),
                                CheckedDate = reader.GetDateTime(3),
                                Description = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }
            return websites;
        }

        public int Create(WebsiteSafety website)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        INSERT INTO WebsiteSafety (Url, IsSafe, CheckedDate, Description)
                        VALUES (@Url, @IsSafe, @CheckedDate, @Description);
                        SELECT SCOPE_IDENTITY();";

                    command.Parameters.AddWithValue("@Url", website.Url);
                    command.Parameters.AddWithValue("@IsSafe", website.IsSafe);
                    command.Parameters.AddWithValue("@CheckedDate", website.CheckedDate);
                    command.Parameters.AddWithValue("@Description", 
                        (object)website.Description ?? DBNull.Value);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool Update(WebsiteSafety website)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        UPDATE WebsiteSafety 
                        SET Url = @Url, 
                            IsSafe = @IsSafe, 
                            CheckedDate = @CheckedDate, 
                            Description = @Description
                        WHERE Id = @Id";

                    command.Parameters.AddWithValue("@Id", website.Id);
                    command.Parameters.AddWithValue("@Url", website.Url);
                    command.Parameters.AddWithValue("@IsSafe", website.IsSafe);
                    command.Parameters.AddWithValue("@CheckedDate", website.CheckedDate);
                    command.Parameters.AddWithValue("@Description", 
                        (object)website.Description ?? DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = _dbConnection.GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM WebsiteSafety WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
} 