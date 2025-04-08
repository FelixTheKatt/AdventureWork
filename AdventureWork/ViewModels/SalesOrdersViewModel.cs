// Usings système et projet
using System.Collections.ObjectModel;              // Pour ObservableCollection<T>
using System.ComponentModel;                      // Pour INotifyPropertyChanged
using System.Runtime.CompilerServices;            // Pour [CallerMemberName]
using System.Windows.Input;                       // Pour ICommand
using AdventureWork.Data;                         // Connexion DB
using AdventureWork.Data.Repositories;            // Repository custom
using AdventureWork.Models;                       // Modèle SalesOrder
using System.Windows.Input;

namespace AdventureWork.ViewModels;

// Le ViewModel est lié à l’UI XAML (SalesOrdersPage.xaml)
// Il expose les données (commandes clients) et gère la recherche.

//Check ObservableProperty => ObservableObject
//check => CommunityToolkit.Mvvm
public class SalesOrdersViewModel : INotifyPropertyChanged
{
    // Liste affichée à l’écran (Observable pour MAUI/XAML)
    private ObservableCollection<SalesOrder> _filteredOrders;

    // Liste source complète (non observable car utilisée en interne uniquement)
    private List<SalesOrder> _allOrders;

    // Champ privé qui stocke la commande sélectionnée
    private SalesOrder? selectedOrder;

    // Propriété publique liée à la sélection dans la CollectionView (SelectedItem binding)
    public SalesOrder? SelectedOrder
    {
        get => selectedOrder;

        set
        {
            // Vérifie que la nouvelle valeur est différente de l'ancienne
            if (selectedOrder != value)
            {
                // Met à jour le champ
                selectedOrder = value;

                // Notifie l'interface que la propriété a changé (nécessaire pour que l’UI suive)
                OnPropertyChanged();

                // Si une commande est sélectionnée (non null), on exécute la navigation
                if (value != null)
                {
                    GoToDetailsCommand.Execute(value);
                }
            }
        }
    }
    public ICommand GoToDetailsCommand { get; }

    // Propriété publique liée au XAML → déclenche OnPropertyChanged
    public ObservableCollection<SalesOrder> SalesOrders
    {
        get => _filteredOrders;
        set
        {
            _filteredOrders = value;
            OnPropertyChanged(); // Notifie la vue MAUI d’un changement
        }
    }

    // Texte de recherche lié à la SearchBar (TwoWay binding)
    public string SearchText { get; set; }

    // Commande liée à la SearchBar (SearchCommand)
    public ICommand SearchCommand { get; }

    // Constructeur : initialisation des données
    public SalesOrdersViewModel()
    {
        // Initialisations strictes → pas de nullable
        _filteredOrders = new ObservableCollection<SalesOrder>();
        _allOrders = new List<SalesOrder>();
        SearchText = string.Empty;

        // Commande associée à la recherche
        SearchCommand = new Command(ApplySearch);

        // Connexion SQL à AdventureWorks via DAL maison
        string connectionString = "Server=FELIX\\SQLEXPRESS;Database=AdventureWorks2022;User Id=maui_user;Password=MauiPass123!;TrustServerCertificate=True;";
        DbConnectionFactory connectionFactory = new DbConnectionFactory(connectionString);
        SalesOrderRepository salesOrderRepository = new SalesOrderRepository(connectionFactory);

        // Récupération des données et stockage
        IEnumerable<SalesOrder> orders = salesOrderRepository.GetAll();
        _allOrders = orders.ToList();

        // Affichage initial = tout
        SalesOrders = new ObservableCollection<SalesOrder>(_allOrders);

        GoToDetailsCommand = new Command<SalesOrder>(order =>
        {
            if (order != null)
            {
                Shell.Current.GoToAsync($"orderdetails?orderId={order.SalesOrderID}");
            }
        });
    }

    // Applique le filtre sur le nom du client
    private void ApplySearch()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            // Si la recherche est vide → on affiche tout
            SalesOrders = new ObservableCollection<SalesOrder>(_allOrders);
        }
        else
        {
            // Sinon → on filtre sur le nom du client (case insensitive)
            var filtered = _allOrders
                .Where(o => o.CustomerName != null &&
                            o.CustomerName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            SalesOrders = new ObservableCollection<SalesOrder>(filtered);
        }
    }

    // === INotifyPropertyChanged ===

    // Événement que le système écoute pour détecter un changement
    public event PropertyChangedEventHandler? PropertyChanged;

    // Méthode utilitaire pour appeler l’événement → lie XAML aux données
    private void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}