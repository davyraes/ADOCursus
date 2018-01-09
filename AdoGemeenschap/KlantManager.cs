using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AdoGemeenschap
{
    public  class KlantManager
    {
        public Int64 NieuweKlant(string naam)
        {
            var manager = new BankDbManager();
            using (var conBank = manager.GetConnection())
            {
                using (var comToevoegen = conBank.CreateCommand())
                {
                    comToevoegen.CommandType = CommandType.StoredProcedure;
                    comToevoegen.CommandText = "NieuweKlant";

                    var parNaam = comToevoegen.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    parNaam.Value = naam;
                    comToevoegen.Parameters.Add(parNaam);

                    conBank.Open();
                    return Convert.ToInt64(comToevoegen.ExecuteScalar());
                }
            }
        }
    }
}
