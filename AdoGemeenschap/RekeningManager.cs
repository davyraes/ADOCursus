using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace AdoGemeenschap
{
    public class RekeningManager
    {
        public Int32 SaldoBonus()
        {
            var dbManager = new BankDbManager();
            using (var conBank = dbManager.GetConnection())
            {
                using (var comBonus = conBank.CreateCommand())
                {
                    comBonus.CommandType = CommandType.Text;
                    comBonus.CommandText = "update Rekeningen set Saldo=Saldo*1.1";
                    conBank.Open();
                    return comBonus.ExecuteNonQuery();
                }
            }
        }
        public Boolean Storten(decimal teStorten, string rekeningNr)
        {
            var dbManager = new BankDbManager();
            using (var conBank = dbManager.GetConnection())
            {
                using (var comStorten = conBank.CreateCommand())
                {
                    comStorten.CommandType = CommandType.StoredProcedure;
                    comStorten.CommandText = "Storten";

                    DbParameter parTeStorten = comStorten.CreateParameter();
                    parTeStorten.ParameterName = "@teStorten";
                    parTeStorten.Value = teStorten;
                    parTeStorten.DbType = DbType.Currency;
                    comStorten.Parameters.Add(parTeStorten);

                    DbParameter parRekeningNr = comStorten.CreateParameter();
                    parRekeningNr.ParameterName = "@RekeningNr";
                    parRekeningNr.Value = rekeningNr;
                    comStorten.Parameters.Add(parRekeningNr);
                    conBank.Open();
                    return comStorten.ExecuteNonQuery() != 0;
                }
            }
        }
        public void Overschrijven(decimal bedrag,string vanRekening,string naarRekening)
        {
            var dbManager = new BankDbManager();
            using (var conBank = dbManager.GetConnection())
            {
                conBank.Open();
                using (var traOverschrijven = conBank.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (var comAftrekken = conBank.CreateCommand())
                    {
                        comAftrekken.Transaction = traOverschrijven;
                        comAftrekken.CommandType = CommandType.Text;
                        comAftrekken.CommandText = "update Rekeningen set Saldo=Saldo-@bedrag where RekeningNr=@reknr";

                        DbParameter parBedrag = comAftrekken.CreateParameter();
                        parBedrag.ParameterName = "@bedrag";
                        parBedrag.Value = bedrag;
                        comAftrekken.Parameters.Add(parBedrag);

                        DbParameter parRekening = comAftrekken.CreateParameter();
                        parRekening.ParameterName = "@reknr";
                        parRekening.Value = vanRekening;

                        if (comAftrekken.ExecuteNonQuery()==0)
                        {
                            traOverschrijven.Rollback();
                            throw new Exception("Van rekening bestaat niet");
                        }
                    }
                    using (var comBijtellen = conBank.CreateCommand())
                    {
                        comBijtellen.Transaction = traOverschrijven;
                        comBijtellen.CommandType = CommandType.StoredProcedure;
                        comBijtellen.CommandText = "Storten";

                        DbParameter parbedrag = comBijtellen.CreateParameter();
                        parbedrag.ParameterName = "@teStorten";
                        parbedrag.Value = bedrag;
                        comBijtellen.Parameters.Add(parbedrag);

                        DbParameter parRekening = comBijtellen.CreateParameter();
                        parRekening.ParameterName = "@rekeningNr";
                        parRekening.Value = naarRekening;
                        comBijtellen.Parameters.Add(parRekening);
                         
                        if (comBijtellen.ExecuteNonQuery()==0)
                        {
                            traOverschrijven.Rollback();
                            throw new Exception("Naar Rekening bestaat niet");
                        }
                    }
                    traOverschrijven.Commit();
                }
            }
        }
    }
}
