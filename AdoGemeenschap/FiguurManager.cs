using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class FiguurManager
    {
        public List<Figuur> GetFiguren()
        {
            List<Figuur> figuren = new List<Figuur>();
            using (var conStrip = StripDbManager.GetConnection())
            {
                using (var comGet = conStrip.CreateCommand())
                {
                    comGet.CommandType = System.Data.CommandType.Text;
                    comGet.CommandText = "select * from figuren";

                    conStrip.Open();
                    using (var rdrStrips = comGet.ExecuteReader())
                    {
                        Int32 IdPos = rdrStrips.GetOrdinal("ID");
                        Int32 NaamPos = rdrStrips.GetOrdinal("Naam");
                        Int32 VersiePos = rdrStrips.GetOrdinal("Versie");

                        while(rdrStrips.Read())
                        {
                            figuren.Add(new Figuur(rdrStrips.GetInt32(IdPos), rdrStrips.GetString(NaamPos), rdrStrips.GetValue(VersiePos)));
                        }
                    }
                }
            }
            return figuren;
        }
        public void SchrijfWijzigingen(List<Figuur> figuren)
        {
            using (var conStrips = StripDbManager.GetConnection())
            {
                using (var comWrite = conStrips.CreateCommand())
                {
                    comWrite.CommandType = System.Data.CommandType.Text;
                    comWrite.CommandText = "update figuren set Naam=@naam where ID=@id and Versie=@versie";

                    var parNaam = comWrite.CreateParameter();
                    parNaam.ParameterName = "@naam";
                    comWrite.Parameters.Add(parNaam);

                    var parID = comWrite.CreateParameter();
                    parID.ParameterName = "@id";
                    comWrite.Parameters.Add(parID);

                    var parVersie = comWrite.CreateParameter();
                    parVersie.ParameterName = "@versie";
                    comWrite.Parameters.Add(parVersie);

                    conStrips.Open();
                    foreach(Figuur f in figuren)
                    {
                        parID.Value = f.ID;
                        parNaam.Value = f.Naam;
                        parVersie.Value = f.Versie;

                        if (comWrite.ExecuteNonQuery() == 0)
                            throw new Exception("Iemand was je voor");
                    }
                }
            }
        } 
    }
}
