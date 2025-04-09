// Usings système et projet
using System.Collections.ObjectModel;              // Pour ObservableCollection<T>
using System.ComponentModel;                      // Pour INotifyPropertyChanged
using System.Runtime.CompilerServices;            // Pour [CallerMemberName]
using System.Windows.Input;                       // Pour ICommand
using AdventureWork.Data;                         // Connexion DB
using AdventureWork.Data.Repositories;            // Repository custom
using AdventureWork.Models;                       // Modèle SalesOrder
using AdventureWork.Configuration;

namespace AdventureWork.ViewModels;

// Le ViewModel est lié à l’UI XAML (SalesOrdersPage.xaml)
// Il expose les données (commandes clients) et gère la recherche.

//Check ObservableProperty => ObservableObject
//check => CommunityToolkit.Mvvm
public class SalesOrdersViewModel : INotifyPropertyChanged
{
    //bloc Pagination
    private int _currentPage = 0;
    private const int _pageSize = 50;
    private int _maxPage = 0;

    private string _pageIndicatorText = string.Empty;
    public string PageIndicatorText
    {
        get => _pageIndicatorText;
        set
        {
            _pageIndicatorText = value;
            OnPropertyChanged();
        }
    }

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

    private bool _isAscending = true;
    public ICommand ToggleSortCommand { get; }

    private void ApplySort()
    {
        var sorted = _isAscending
            ? _filteredOrders.OrderBy(o => o.TotalDue).ToList()
            : _filteredOrders.OrderByDescending(o => o.TotalDue).ToList();

        SalesOrders = new ObservableCollection<SalesOrder>(sorted);
        SortButtonText = _isAscending ? "Trier  total ↓" : "Trier total ↑";
        _isAscending = !_isAscending; // Inverse clic true false
    }

    private string _sortButtonText = "Trier total ↓";
    public string SortButtonText
    {
        get => _sortButtonText;
        set
        {
            _sortButtonText = value;
            OnPropertyChanged();
        }
    }

    // Icommand pagination
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    // Constructeur : A refactor trop de truc dedans
    public SalesOrdersViewModel()
    {
        // Initialisations strictes → pas de nullable avec ? a la typescript
        _filteredOrders = new ObservableCollection<SalesOrder>();
        _allOrders = new List<SalesOrder>();
        SearchText = string.Empty;

        // Commande associée à la recherche
        SearchCommand = new Command(ApplySearch);

        // Connexion SQL à AdventureWorks via DAL maison
        DbConnectionFactory connectionFactory = new DbConnectionFactory(DbSettings.ConnectionString);
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

        ToggleSortCommand = new Command(ApplySort);
        SortButtonText = "Trier total ↓";


        // Nouvelle méthode pour récupérer une page refactor
        // créer methode => KISS
        // Charger toutes les commandes dans la liste complète
        _allOrders = salesOrderRepository.GetPage(_currentPage, _pageSize).ToList();
        _maxPage = (int)Math.Ceiling((double)salesOrderRepository.CountAll() / _pageSize);

        LoadPage();

        NextPageCommand = new Command(NextPage);
        PreviousPageCommand = new Command(PreviousPage);
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

    private void NextPage()
    {
        if (_currentPage < _maxPage - 1)
        {
            _currentPage++;
            LoadPage();
        }
    }

    private void PreviousPage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            LoadPage();
        }
    }

    private void LoadPage()
    {
        DbConnectionFactory connectionFactory = new DbConnectionFactory(DbSettings.ConnectionString);
        SalesOrderRepository salesOrderRepository = new SalesOrderRepository(connectionFactory);

        var orders = salesOrderRepository.GetPage(_currentPage, _pageSize).ToList();
        SalesOrders = new ObservableCollection<SalesOrder>(orders);

        int totalCount = salesOrderRepository.CountAll();
        _maxPage = (int)Math.Ceiling((double)totalCount / _pageSize);
        PageIndicatorText = $"Page {_currentPage + 1} / {_maxPage}";
    }
}