using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoGemeenschap
{
    public class Genre
    {
        public Genre(int nGenreNr,string nNaam)
        {
            this.GenreNr = nGenreNr;
            this.Naam = nNaam;
        }
        public int GenreNr { get; set; }
        public string Naam { get; set; }
    }
}
