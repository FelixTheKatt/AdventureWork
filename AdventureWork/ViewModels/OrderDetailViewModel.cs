
using System.Collections.ObjectModel;
using AdventureWork.Configuration;
using AdventureWork.Data;
using AdventureWork.Data.Repositories;
using AdventureWork.Models;

namespace AdventureWork.ViewModels
{
    public class OrderDetailViewModel
    {
        public ObservableCollection<OrderDetailLine> Lines { get; set; }
        public SalesOrder SelectedOrder { get; set; }
        public decimal SubTotal => Lines.Sum(line => line.LineTotal);

        public OrderDetailViewModel(int orderId)
        {
            Lines = new ObservableCollection<OrderDetailLine>();

            // Connexion SQL
            DbConnectionFactory connectionFactory = new DbConnectionFactory(DbSettings.ConnectionString);
            OrderDetailRepository repository = new OrderDetailRepository(connectionFactory);
            SalesOrderRepository orderRepo = new SalesOrderRepository(connectionFactory);

            // Récupération des lignes
            var details = repository.GetByOrderId(orderId);

            foreach (OrderDetailLine line in details)
            {
                Lines.Add(line);
            }

            var order = orderRepo.GetById(orderId);
            SelectedOrder = orderRepo.GetById(orderId);
        }
    }
}
