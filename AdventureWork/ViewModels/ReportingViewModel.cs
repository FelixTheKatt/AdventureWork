using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AdventureWork.Models;
using AdventureWork.Data;
using AdventureWork.Data.Repositories;
using AdventureWork.Configuration;


namespace AdventureWork.ViewModels
{
    public class ReportingViewModel 
    {
        private readonly ReportingRepository _repo;

        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageBasket => TotalOrders > 0 ? TotalRevenue / TotalOrders : 0;

        public ObservableCollection<string> TopCustomers { get; set; } = new();

        public string TotalOrdersFormatted => $"Commandes : {TotalOrders}";
        public string TotalRevenueFormatted => $"Ventes totales : {TotalRevenue:C}";
        public string AverageOrderFormatted => $"Panier moyen : {AverageBasket:C}";

        public ReportingViewModel()
        {
            var factory = new DbConnectionFactory(DbSettings.ConnectionString);
            _repo = new ReportingRepository(factory);

            TotalOrders = _repo.GetTotalOrders();
            TotalRevenue = _repo.GetTotalRevenue();

            var top = _repo.GetTopCustomers(5);
            foreach (var customer in top)
                TopCustomers.Add($"{customer.CustomerName} : {customer.Total:C}");
        }
    }
}
