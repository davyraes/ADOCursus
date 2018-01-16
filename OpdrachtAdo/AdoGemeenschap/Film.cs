using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class Film
    {
        public Film()
        {
            this.bandNrValue= 0;
            this.Titel = string.Empty;
            this.GenreNr = 0;
            this.InVoorraad = 0;
            this.UitVoorraad = 0;
            this.Prijs = 0m;
            this.TotaalVerhuurd = 0;
            Changed = false;
        }
        public Film(int nBandnr,string nTitel, int nGenreNr,int nInVoorraad,int nUitVoorraad,decimal nPrijs,int nTotaalVerhuurd)
        {
            this.bandNrValue = nBandnr;
            this.Titel = nTitel;
            this.GenreNr = nGenreNr;
            this.InVoorraad = nInVoorraad;
            this.UitVoorraad= nUitVoorraad;
            this.Prijs = nPrijs;
            this.TotaalVerhuurd = nTotaalVerhuurd;
            this.Changed = false;
        }
        private int bandNrValue;
        private string titelValue;
        private int genreNrValue;
        private int inVoorraadValue;
        private int uitVoorraadValue;
        private decimal prijsValue;
        private int totaalVerhuurdValue;

        public bool Changed { get; set; }

        public int BandNr
        {
            get
            {
                return bandNrValue;
            }
        }
        public string Titel
        {
            get { return titelValue; }
            set { titelValue = value;Changed = true; }
        }
        public int GenreNr
        {
            get { return genreNrValue; }
            set { genreNrValue = value;Changed = true; }
        }
        public int InVoorraad
        {
            get { return inVoorraadValue; }
            set { inVoorraadValue = value;Changed = true; }
        }
        public int UitVoorraad
        {
            get { return uitVoorraadValue; }
            set { uitVoorraadValue = value;Changed = true; }
        }
        public decimal Prijs
        {
            get { return prijsValue; }
            set { prijsValue = value;Changed = true; }
        }
        public int TotaalVerhuurd
        {
            get { return totaalVerhuurdValue; }
            set { totaalVerhuurdValue = value;Changed = true; }
        }
        public void verhuren()
        {
            if (this.InVoorraad <= 0)
                throw new Exception("Alle Films zijn verhuurd !");
            InVoorraad -= 1;
            UitVoorraad += 1;
            totaalVerhuurdValue += 1;
        }
        public void TerugBrengen()
        {
            if (uitVoorraadValue <= 0)
                throw new Exception("Alle films zijn reeds in voorraad controleer stock !!!!");
            InVoorraad += 1;
            UitVoorraad -= 1;
        }
    }
}
