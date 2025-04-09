namespace AdventureWork.Configuration
{
    public static class AppConfig
    {
        public const string LocalConnectionString =
            "Server=FELIX\\SQLEXPRESS;Database=AdventureWorks2022;User Id=maui_user;Password=MauiPass123!;TrustServerCertificate=True;";

        public const string AzureConnectionString =
            "Server=tcp:sql-demo-felix.database.windows.net,1433;Initial Catalog=AdventureWorks2022;User ID=sqladmin;Password=NouveauMotDePasse!123;Encrypt=True;TrustServerCertificate=true;";

    }
}

