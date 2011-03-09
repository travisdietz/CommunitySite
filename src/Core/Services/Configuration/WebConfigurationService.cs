using System.Configuration;

namespace CommunitySite.Core.Services.Configuration
{
    public class WebConfigurationService : ConfigurationService
    {
        public string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["default"].ConnectionString; }
        }
    }
}