
using AdventureWork.ViewModels;

namespace AdventureWork.Views;

[QueryProperty(nameof(OrderId), "orderId")]
public partial class OrderDetailPage : ContentPage
{
    public int OrderId { get; set; }

    public OrderDetailPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // On crée et injecte le ViewModel seulement maintenant, quand la navigation est finie
        BindingContext = new OrderDetailViewModel(OrderId);
    }
}