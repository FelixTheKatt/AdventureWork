using AdventureWork.Data.Repositories;
using AdventureWork.Data;
using AdventureWork.Models;
using System.Diagnostics;

namespace AdventureWork
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            TestSalesOrderRepository();
        }

        private void TestSalesOrderRepository()
        {
            string connectionString = "Server=FELIX\\SQLEXPRESS;Database=AdventureWorks2022;User Id=maui_user;Password=MauiPass123!;TrustServerCertificate=True;";
            DbConnectionFactory connectionFactory = new DbConnectionFactory(connectionString);
            SalesOrderRepository salesOrderRepository = new SalesOrderRepository(connectionFactory);

            IEnumerable<SalesOrder> orders = salesOrderRepository.GetAll();

            foreach (SalesOrder order in orders)
            {
                Debug.WriteLine($"{order.SalesOrderID} | {order.CustomerName} | {order.OrderDate.ToShortDateString()} | {order.TotalDue:C}");
            }
        }
    }

}
