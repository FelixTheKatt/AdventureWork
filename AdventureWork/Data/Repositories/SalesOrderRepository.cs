using AdventureWork.Models;
using Microsoft.Data.SqlClient;

namespace AdventureWork.Data.Repositories;

public class SalesOrderRepository : RepositoryBase<SalesOrder>
{
    public SalesOrderRepository(DbConnectionFactory factory)
        : base(factory) { }

    public override IEnumerable<SalesOrder> GetAll()
    {
        SqlConnection conn = ConnectionFactory.CreateConnection();

        SqlCommand command = new SqlCommand(@"
            SELECT TOP 50 
                h.SalesOrderID, 
                h.OrderDate, 
                CONCAT(p.FirstName, ' ', p.LastName) as CustomerName, 
                h.TotalDue
            FROM Sales.SalesOrderHeader h
            JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
            JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
            ORDER BY h.OrderDate DESC", conn);

        List<SalesOrder> orders = new List<SalesOrder>();

        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            SalesOrder order = new SalesOrder
            {
                SalesOrderID = reader.GetInt32(0),
                OrderDate = reader.GetDateTime(1),
                CustomerName = reader.GetString(2),
                TotalDue = reader.GetDecimal(3)
            };

            orders.Add(order);
        }

        reader.Close();
        conn.Close();

        return orders;
    }

    public override SalesOrder GetById(int id)
    {
        throw new NotImplementedException();
    }

    public override void Add(SalesOrder entity)
    {
        throw new NotImplementedException();
    }

    public override void Update(SalesOrder entity)
    {
        throw new NotImplementedException();
    }

    public override void Delete(int id)
    {
        throw new NotImplementedException();
    }
}