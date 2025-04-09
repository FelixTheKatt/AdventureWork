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

        int pageNumber = 0; 
        int pageSize = 100;

        // => OPTIMISER CAR 31465 ENTRIES dans la db
        //garder le top 50 pour le moment
        //galère je connais pas bien la db check en detail dans ssms optimiser query
        //pagination ? manuel ???
        SqlCommand command = new SqlCommand(@"
            SELECT 
                h.SalesOrderID, 
                h.OrderDate, 
                CONCAT(p.FirstName, ' ', p.LastName) as CustomerName, 
                h.TotalDue
            FROM Sales.SalesOrderHeader h
            JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
            JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
            ORDER BY h.OrderDate DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY", conn);

        //test check si cela passe a la main
        command.Parameters.AddWithValue("@Offset", pageNumber * pageSize);
        command.Parameters.AddWithValue("@PageSize", pageSize);

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
        using SqlConnection conn = ConnectionFactory.CreateConnection();

        var cmd = new SqlCommand(@"
        SELECT 
            h.SalesOrderID,
            h.OrderDate,
            h.DueDate,
            h.ShipDate,
            h.Status,
            h.AccountNumber,
            h.SubTotal,
            h.TaxAmt,
            h.Freight,
            h.TotalDue,
            s.Name AS ShipMethod,
            CONCAT(p.FirstName, ' ', p.LastName) AS CustomerName
        FROM Sales.SalesOrderHeader h
        JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
        JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
		JOIN Purchasing.ShipMethod s ON h.ShipMethodID = s.ShipMethodID
        WHERE h.SalesOrderID = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new SalesOrder
            {
                SalesOrderID = reader.GetInt32(0),
                OrderDate = reader.GetDateTime(1),
                DueDate = reader.GetDateTime(2),
                ShipDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                Status = reader.GetByte(4),
                AccountNumber = reader.GetString(5),
                SubTotal = reader.GetDecimal(6),
                TaxAmt = reader.GetDecimal(7),
                Freight = reader.GetDecimal(8),
                TotalDue = reader.GetDecimal(9),
                ShipMethod = reader.GetString(10),
                CustomerName = reader.GetString(11)
            };
        }

        return null;
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

    public IEnumerable<SalesOrder> GetPage(int pageNumber, int pageSize)
    {
        SqlConnection conn = ConnectionFactory.CreateConnection();

        var cmd = new SqlCommand(@"
        SELECT 
            h.SalesOrderID,
            h.OrderDate,
            CONCAT(p.FirstName, ' ', p.LastName) AS CustomerName,
            h.TotalDue
        FROM Sales.SalesOrderHeader h
        JOIN Sales.Customer c ON h.CustomerID = c.CustomerID
        JOIN Person.Person p ON c.PersonID = p.BusinessEntityID
        ORDER BY h.OrderDate DESC
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY", conn);

        cmd.Parameters.AddWithValue("@Offset", pageNumber * pageSize);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);

        using var reader = cmd.ExecuteReader();
        var orders = new List<SalesOrder>();

        while (reader.Read())
        {
            orders.Add(new SalesOrder
            {
                SalesOrderID = reader.GetInt32(0),
                OrderDate = reader.GetDateTime(1),
                CustomerName = reader.GetString(2),
                TotalDue = reader.GetDecimal(3)
            });
        }

        return orders;
    }

    public int CountAll()
    {
        SqlConnection conn = ConnectionFactory.CreateConnection();

        var cmd = new SqlCommand("SELECT COUNT(*) FROM Sales.SalesOrderHeader", conn);
        return (int)cmd.ExecuteScalar();
    }
}