
using System.Collections.ObjectModel;
using AdventureWork.Data;
using AdventureWork.Data.Repositories;
using AdventureWork.Models;

namespace AdventureWork.ViewModels
{
    public class OrderDetailViewModel
    {
        public ObservableCollection<OrderDetailLine> Lines { get; set; }

        public OrderDetailViewModel(int orderId)
        {
            Lines = new ObservableCollection<OrderDetailLine>();

            // Connexion SQL
            string connectionString = "Server=FELIX\\SQLEXPRESS;Database=AdventureWorks2022;User Id=maui_user;Password=MauiPass123!;TrustServerCertificate=True;";
            DbConnectionFactory factory = new DbConnectionFactory(connectionString);
            OrderDetailRepository repository = new OrderDetailRepository(factory);

            // Récupération des lignes
            var details = repository.GetByOrderId(orderId);

            foreach (OrderDetailLine line in details)
            {
                Lines.Add(line);
            }
        }
    }
}
