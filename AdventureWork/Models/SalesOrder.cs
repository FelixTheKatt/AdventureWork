
namespace AdventureWork.Models
{
    public class SalesOrder
    {
        public int SalesOrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public byte Status { get; set; }
        public string? StatusText => Status switch
        {
            1 => "En cours",
            2 => "Approuvée",
            3 => "Complétée",
            4 => "Annulée",
            _ => "Inconnue"
        };
        public string? CustomerName { get; set; }
        public string? AccountNumber { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Freight { get; set; }
        public decimal TotalDue { get; set; }
        public string? ShipMethod { get; set; }
    }
}
