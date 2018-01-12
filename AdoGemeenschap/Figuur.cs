using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdoGemeenschap
{
    public class Figuur
    {
        private Int32 IDValue;
        private string naamValue;
        private object versieValue;

        public Int32 ID
        {
            get { return IDValue; }
            set { IDValue = value; }
        }
        public string Naam
        {
            get { return naamValue; }
            set { naamValue = value;Changed = true; }
        }
        public object Versie
        {
            get { return versieValue; }
            set { versieValue = value; }
        }
        public bool Changed { get; set; }
        public Figuur(Int32 nId,string nNaam,object nVersie)
        {
            ID = nId;
            Naam = nNaam;
            Versie = nVersie;
            Changed = false;
        }
        public Figuur()
        {
            Changed = false;
        }
    }
}
