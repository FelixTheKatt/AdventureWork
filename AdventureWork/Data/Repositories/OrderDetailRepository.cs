using AdventureWork.Models;
using Microsoft.Data.SqlClient;

namespace AdventureWork.Data.Repositories
{
    public class OrderDetailRepository
    {
        private readonly DbConnectionFactory connectionFactory;

        public OrderDetailRepository(DbConnectionFactory factory)
        {
            connectionFactory = factory;
        }

        public List<OrderDetailLine> GetByOrderId(int orderId)
        {
            List<OrderDetailLine> details = new();

            using SqlConnection connection = connectionFactory.CreateConnection();
            connection.Open();

            string query = @"
            SELECT p.Name, sod.OrderQty, sod.UnitPrice
            FROM Sales.SalesOrderDetail sod
            INNER JOIN Production.Product p ON p.ProductID = sod.ProductID
            WHERE sod.SalesOrderID = @OrderId";

            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OrderId", orderId);

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                details.Add(new OrderDetailLine
                {
                    ProductName = reader.GetString(0),
                    Quantity = reader.GetInt16(1),
                    UnitPrice = reader.GetDecimal(2)
                });
            }

            return details;
        }
    }
}
