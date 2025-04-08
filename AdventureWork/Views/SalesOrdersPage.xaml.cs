using AdventureWork.ViewModels;


namespace AdventureWork.Views;

public partial class SalesOrdersPage : ContentPage
{
    private SalesOrdersViewModel viewModel;

    public SalesOrdersPage()
    {
        InitializeComponent();

        viewModel = new SalesOrdersViewModel();
        BindingContext = viewModel;

        // Abonnement au message de scroll
        MessagingCenter.Subscribe<SalesOrdersViewModel>(this, "ScrollToTop", (sender) =>
        {
            ScrollToTop();
        });
    }

    private void ScrollToTop()
    {
        if (OrdersCollection.ItemsSource is IEnumerable<object> items && items.Any())
        {
            OrdersCollection.ScrollTo(items.First(), position: ScrollToPosition.Start, animate: false);
        }
    }
}