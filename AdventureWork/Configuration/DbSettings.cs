namespace AdventureWork.Configuration
{
    public static class DbSettings
    {
        public static bool UseAzure = true;
        public static string ConnectionString =>
            UseAzure
                ? AppConfig.AzureConnectionString
                : AppConfig.LocalConnectionString;
    }
}
