using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace AdoGemeenschap
{
    public class BrouwerManager
    {
        public ObservableCollection<Brouwer> GetBrouwersBeginNaam(string beginNaam)
        {
            ObservableCollection<Brouwer> brouwers = new ObservableCollection<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                using (var comBrouwers = conBieren.CreateCommand())
                {
                    comBrouwers.CommandType = CommandType.Text;
                    if (beginNaam != string.Empty)
                    {
                        comBrouwers.CommandText = "select * from Brouwers where BrNaam like @zoals order by brnaam";
                        var parZoals = comBrouwers.CreateParameter();
                        parZoals.ParameterName = "@zoals";
                        parZoals.Value = beginNaam+"%";
                        comBrouwers.Parameters.Add(parZoals);
                    }
                    else
                        comBrouwers.CommandText = "select * from brouwers";
                    conBieren.Open();
                    using (var rdrBrouwers = comBrouwers.ExecuteReader())
                    {
                        Int32 brouwerNrPos = rdrBrouwers.GetOrdinal("BrouwerNr");
                        Int32 brNaamPos = rdrBrouwers.GetOrdinal("brnaam");
                        Int32 adresPos = rdrBrouwers.GetOrdinal("adres");
                        Int32 postcodePos = rdrBrouwers.GetOrdinal("postcode");
                        Int32 gemeentePos = rdrBrouwers.GetOrdinal("gemeente");
                        Int32 omzetPos = rdrBrouwers.GetOrdinal("Omzet");

                        Int32? omzet;
                        while (rdrBrouwers.Read())
                        {
                            if (rdrBrouwers.IsDBNull(omzetPos))
                                omzet = null; 
                            else
                                omzet = rdrBrouwers.GetInt32(omzetPos);
                            brouwers.Add(new Brouwer(
                                rdrBrouwers.GetInt32(brouwerNrPos),
                                rdrBrouwers.GetString(brNaamPos),
                                rdrBrouwers.GetString(adresPos),
                                rdrBrouwers.GetInt16(postcodePos),
                                rdrBrouwers.GetString(gemeentePos),
                                omzet
                                ));

                        }
                        return brouwers;
                    }
                }
            }
        }
        public List<Brouwer>SchrijfVerwijderingen(List<Brouwer>brouwers)
        {
            List<Brouwer> nietVerwijderdeBrouwers = new List<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                using (var comDelete = conBieren.CreateCommand())
                {
                    comDelete.CommandType = CommandType.Text;
                    comDelete.CommandText = "delete from brouwers where BrouwerNr = @brouwernr";

                    var parBrouwerNr = comDelete.CreateParameter();
                    parBrouwerNr.ParameterName = "@brouwernr";
                    comDelete.Parameters.Add(parBrouwerNr);
                    conBieren.Open();
                    foreach(Brouwer eenBrouwer in brouwers)
                    {
                        try
                        {
                            parBrouwerNr.Value = eenBrouwer.BrouwersNr;
                            if (comDelete.ExecuteNonQuery() == 0)
                                nietVerwijderdeBrouwers.Add(eenBrouwer);
                        }
                        catch(Exception)
                        {
                            nietVerwijderdeBrouwers.Add(eenBrouwer);
                        }
                    }
                }
            }
            return nietVerwijderdeBrouwers;
        }
        public List<Brouwer>SchrijfToevoegingen(List<Brouwer>brouwers)
        {
            List<Brouwer> nietToegevoegdeBrouwers = new List<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                using (var comToevoegen = conBieren.CreateCommand())
                {
                    comToevoegen.CommandType = CommandType.Text;
                    comToevoegen.CommandText = "insert into brouwers(brNaam,adres,postcode,gemeente,omzet)values(@brnaam,@adres,@postcode,@gemeente,@omzet)";

                    var parBrNaam = comToevoegen.CreateParameter();
                    parBrNaam.ParameterName = "@naam";
                    comToevoegen.Parameters.Add(parBrNaam);

                    var parAdres = comToevoegen.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comToevoegen.Parameters.Add(parAdres);

                    var parPostCode = comToevoegen.CreateParameter();
                    parPostCode.ParameterName = "@postcode";
                    comToevoegen.Parameters.Add(parPostCode);

                    var parGemeente = comToevoegen.CreateParameter();
                    parGemeente.ParameterName = "@gemeente";
                    comToevoegen.Parameters.Add(parGemeente);

                    var parOmzet = comToevoegen.CreateParameter();
                    parOmzet.ParameterName = "@omzet";
                    comToevoegen.Parameters.Add(parOmzet);

                    conBieren.Open();
                    foreach(Brouwer eenBrouwer in brouwers)
                    {
                        try
                        {
                            parBrNaam.Value = eenBrouwer.BrNaam;
                            parAdres.Value = eenBrouwer.Adres;
                            parPostCode.Value = eenBrouwer.Postcode;
                            parGemeente.Value = eenBrouwer.Gemeente;
                            if (eenBrouwer.Omzet.HasValue)
                                parOmzet.Value = eenBrouwer.Omzet;
                            else
                                parOmzet.Value = DBNull.Value;
                            if (comToevoegen.ExecuteNonQuery() == 0)
                                nietToegevoegdeBrouwers.Add(eenBrouwer);
                        }
                        catch(Exception)
                        {
                            nietToegevoegdeBrouwers.Add(eenBrouwer);

                        }
                    }

                }
            }
            return nietToegevoegdeBrouwers;
        }
        public List<Brouwer>SchrijfWijzigingen(List<Brouwer> brouwers)
        {
            List<Brouwer> nietDoorgevoerdeBrouwers = new List<Brouwer>();
            var manager = new BierenDbManager();
            using (var conBieren = manager.GetConnection())
            {
                using (var comUpdate = conBieren.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "update brouwers set brnaam=@brnaam,adres = @adres,postcode=@postcode,gemeente=@gemeente,omzet=@omzet where brouwernr=@brouwernr";

                    var parBrNaam = comUpdate.CreateParameter();
                    parBrNaam.ParameterName = "@naam";
                    comUpdate.Parameters.Add(parBrNaam);

                    var parAdres = comUpdate.CreateParameter();
                    parAdres.ParameterName = "@adres";
                    comUpdate.Parameters.Add(parAdres);

                    var parPostCode = comUpdate.CreateParameter();
                    parPostCode.ParameterName = "@postcode";
                    comUpdate.Parameters.Add(parPostCode);

                    var parGemeente = comUpdate.CreateParameter();
                    parGemeente.ParameterName = "@gemeente";
                    comUpdate.Parameters.Add(parGemeente);

                    var parOmzet = comUpdate.CreateParameter();
                    parOmzet.ParameterName = "@omzet";
                    comUpdate.Parameters.Add(parOmzet);

                    var parBrNr = comUpdate.CreateParameter();
                    parBrNr.ParameterName = "@brouwernr";
                    comUpdate.Parameters.Add(parBrNr);

                    conBieren.Open();
                    foreach(Brouwer eenbrouwer in brouwers)
                    {
                        try
                        {
                            parBrNaam.Value = eenbrouwer.BrNaam;
                            parAdres.Value = eenbrouwer.Adres;
                            parPostCode.Value = eenbrouwer.Postcode;
                            parGemeente.Value = eenbrouwer.Gemeente;
                            if (eenbrouwer.Omzet.HasValue)
                                parOmzet.Value = eenbrouwer.Omzet;
                            else
                                parOmzet.Value = DBNull.Value;
                            parBrNr.Value = eenbrouwer.BrouwersNr;
                            if (comUpdate.ExecuteNonQuery() == 0)
                                nietDoorgevoerdeBrouwers.Add(eenbrouwer);                           
                        }
                        catch(Exception)
                        {
                            nietDoorgevoerdeBrouwers.Add(eenbrouwer);
                        }
                    }

                }
            }
            return nietDoorgevoerdeBrouwers;
        }
    }
}
