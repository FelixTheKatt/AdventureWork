using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
