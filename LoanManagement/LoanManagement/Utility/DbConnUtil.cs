using Microsoft.Extensions.Configuration;

namespace LoanManagement.Utility
{
    internal class DbConnUtil
    {
        private static IConfiguration _iconfiguration;
        static DbConnUtil()
        {
            GetAppSettingsFile();
        }

        private static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();
        }
        public static string GetConnString()
        {
            return _iconfiguration.GetConnectionString("LocalConnectionString");
        }      
    }
}
