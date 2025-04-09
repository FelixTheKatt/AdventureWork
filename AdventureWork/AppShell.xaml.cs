using AdventureWork.Views;

namespace AdventureWork
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            // Enregistrement d'une route nommée "orderdetails"
            // Cela permet de naviguer vers une page avec : Shell.Current.GoToAsync("orderdetails?orderId=123")
            // C'est équivalent à Angular : { path: 'orderdetails/:orderId', component: OrderDetailComponent }
            // Ici, on passe les paramètres via query string au lieu des segments d'URL
            //faire Route et subrouting dans une class a part et centraliser puis injecter ici ????
            Routing.RegisterRoute("orderdetails", typeof(OrderDetailPage));
            Routing.RegisterRoute("reporting", typeof(ReportingPage));
        }
    }
}
