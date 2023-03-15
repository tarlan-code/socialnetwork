using Microsoft.Extensions.Configuration;

namespace Zust.Persistence
{
    static class Configuration
    {

        static void InjectJson(out ConfigurationManager conf)
        {
            ConfigurationManager configuration = new();
            configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/Zust.Web"));
            configuration.AddJsonFile("appsettings.json");
            conf = configuration;
        }
        public static string ConnectionString
        {
            get
            {
                InjectJson(out ConfigurationManager conf);
                return conf.GetConnectionString("MSSQL");
            }
        }

        public static string GoogleClientId
        {
            get
            {
                InjectJson(out ConfigurationManager conf);
                return conf["GoogleAuth:ClientID"];
            }
        }


        public static string GoogleSecret
        {
            get
            {
                InjectJson(out ConfigurationManager conf);
                return conf["GoogleAuth:ClientSecret"];
            }
        }


    }
}
