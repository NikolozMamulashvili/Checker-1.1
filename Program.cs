using System;

namespace WebsiteSafetyChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //connection string chaanacvle 
            string connectionString = "123";
            
            var dbConnection = new DatabaseConnection(connectionString);
            dbConnection.InitializeDatabase();
            
            var crud = new CRUD(dbConnection);
        }
    }
} 