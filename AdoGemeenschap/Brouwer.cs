using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class Brouwer
    {
        private Int32 brouwersNrValue;
        private string brNaamValue;
        private string adresValue;
        private Int16 postcodeValue;
        private string gemeenteValue;
        private Int32? omzetValue;

        public bool changed { get; set; }

        public int BrouwersNr { get { return brouwersNrValue; } }
        public string BrNaam { get { return brNaamValue; } set { brNaamValue = value;changed = true; } }
        public string Adres { get { return adresValue; } set { adresValue = value;changed = true; } }
        public Int16 Postcode
        {
            get { return postcodeValue; }
            set { postcodeValue = value;changed = true; }
        }
        public string Gemeente { get { return gemeenteValue; } set { gemeenteValue = value;changed = true; } }
        public Int32? Omzet
        {
            get { return omzetValue; }
            set
            {
                if (value.HasValue && Convert.ToInt32(value) < 0)
                    throw new Exception("Omzet moet positief zijn");
                omzetValue = value;
                changed = true;
            }
        }
        public Brouwer(Int32 brNr,string brNaam,string adres,Int16 postcode,string gemeente,Int32? omzet)
        {
            brouwersNrValue = brNr;
            this.BrNaam = brNaam;
            this.Adres = adres;
            this.Postcode = postcode;
            this.Gemeente = gemeente;
            this.Omzet = omzet;
            this.changed = false;
        }
        public Brouwer() { }
    }
}
