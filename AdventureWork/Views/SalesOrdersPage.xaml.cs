using AdventureWork.ViewModels;

namespace AdventureWork.Views;

public partial class SalesOrdersPage : ContentPage
{
	public SalesOrdersPage()
	{
		InitializeComponent();
        BindingContext = new SalesOrdersViewModel();
    }
}