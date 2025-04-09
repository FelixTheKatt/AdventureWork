using AdventureWork.ViewModels;


namespace AdventureWork.Views;

public partial class SalesOrdersPage : ContentPage
{
    private SalesOrdersViewModel viewModel;

	public SalesOrdersPage()
	{
		InitializeComponent();
        BindingContext = new SalesOrdersViewModel();
    }
}