using Microsoft.Data.SqlClient;

namespace AdventureWork.Data
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            try
            {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
            catch (SqlException ex)
            {
                
                Console.WriteLine($"Erreur de connexion à la base de données : {ex.Message}");
                throw; // Relance l'exception si tu veux la propager
            }
        }
    }
}
