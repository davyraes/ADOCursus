using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class RekeningInfo
    {
        private decimal saldovalue;

        public decimal Saldo
        {
            get { return saldovalue; }
            set { saldovalue = value; }
        }

        private string klantNaamValue;

        public string Klantnaam
        {
            get { return klantNaamValue; }
            set { klantNaamValue = value; }
        }
        public RekeningInfo(decimal nSaldo,string nKlantnaam)
        {
            Saldo = nSaldo;
            Klantnaam = nKlantnaam;
        }

    }
}
