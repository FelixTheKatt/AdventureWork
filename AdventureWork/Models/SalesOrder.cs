
namespace AdventureWork.Models
{
    public class SalesOrder
    {
        public int SalesOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public decimal TotalDue { get; set; }
    }
}
