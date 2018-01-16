using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;

namespace AdoGemeenschap
{
    public static class VideotheekDbManager
    {
        private static ConnectionStringSettings convideotheekSetting = ConfigurationManager.ConnectionStrings["videotheek"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(convideotheekSetting.ProviderName);

        public static DbConnection GetConnection()
        {
            DbConnection videocon = factory.CreateConnection();
            videocon.ConnectionString = convideotheekSetting.ConnectionString;
            return videocon;
        }
    }
}
