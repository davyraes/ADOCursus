using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Configuration;

namespace AdoGemeenschap
{
    public static class StripDbManager
    {
        private static ConnectionStringSettings ConStripSetting = ConfigurationManager.ConnectionStrings["Strips"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(ConStripSetting.ProviderName);
        public static DbConnection GetConnection()
        {
            var conStrip = factory.CreateConnection();
            conStrip.ConnectionString = ConStripSetting.ConnectionString;
            return conStrip;
        }
    }
}
