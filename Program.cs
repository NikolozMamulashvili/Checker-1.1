using System;

namespace WebsiteSafetyChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //chaanacvlet connection string 
            string connectionString = "123";
            
            var dbConnection = new DatabaseConnection(connectionString);
            dbConnection.InitializeDatabase();
            
            var crud = new CRUD(dbConnection);
        }
    }
} 
