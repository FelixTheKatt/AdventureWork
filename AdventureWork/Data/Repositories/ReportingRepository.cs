using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace AdventureWork.Data.Repositories
{
    public class ReportingRepository
    {
        private readonly DbConnectionFactory _factory;

        public ReportingRepository(DbConnectionFactory factory)
        {
            _factory = factory;
        }

        public int GetTotalOrders()
        {
            using var conn = _factory.CreateConnection();
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM Sales.SalesOrderHeader", conn);
            return (int)cmd.ExecuteScalar();
        }

        public decimal GetTotalRevenue()
        {
            using var conn = _factory.CreateConnection();
            using var cmd = new SqlCommand("SELECT SUM(TotalDue) FROM Sales.SalesOrderHeader", conn);
            return (decimal)cmd.ExecuteScalar();
        }

        public List<(string CustomerName, decimal Total)> GetTopCustomers(int topN)
        {
            var result = new List<(string, decimal)>();

            using var conn = _factory.CreateConnection();
            var cmd = new SqlCommand(@"
            SELECT TOP (@TopN)
                CONCAT(p.FirstName, ' ', p.LastName) AS CustomerName,
                SUM(h.TotalDue) AS Total
            FROM Sales.SalesOrderHeader h
            JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
            JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
            GROUP BY p.FirstName, p.LastName
            ORDER BY Total DESC", conn);

            cmd.Parameters.AddWithValue("@TopN", topN);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add((reader.GetString(0), reader.GetDecimal(1)));
            }

            return result;
        }
    }
}
