using AdventureWork.ViewModels;

namespace AdventureWork.Views;

public partial class ReportingPage : ContentPage
{
	public ReportingPage()
	{
		InitializeComponent();
        BindingContext = new ReportingViewModel();
    }
}