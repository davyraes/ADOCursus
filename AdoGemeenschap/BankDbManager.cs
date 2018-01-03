using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;

namespace AdoGemeenschap
{
    public class BankDbManager
    {
        private static ConnectionStringSettings conBankSetting = ConfigurationManager.ConnectionStrings["Bank"];
        private static DbProviderFactory factory = DbProviderFactories.GetFactory(conBankSetting.ProviderName);
        public DbConnection GetConnection()
        {
            var conBank = factory.CreateConnection();
            conBank.ConnectionString = conBankSetting.ConnectionString;
            return conBank;
        }
    }
}
