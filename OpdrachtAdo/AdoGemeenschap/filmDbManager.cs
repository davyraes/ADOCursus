using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace AdoGemeenschap
{
    public class filmDbManager
    {
        public static ObservableCollection<Film> FilmsOphalen()
        {
            ObservableCollection<Film> films = new ObservableCollection<Film>();
            using (var conVideo = VideotheekDbManager.GetConnection())
            {
                using (var comInfo = conVideo.CreateCommand())
                {
                    comInfo.CommandType = CommandType.StoredProcedure;
                    comInfo.CommandText = "FilmsOphalen";

                    conVideo.Open();
                    using (var rdrFilm = comInfo.ExecuteReader())
                    {
                        Int32 posBandNr = rdrFilm.GetOrdinal("BandNr");
                        Int32 posTitel = rdrFilm.GetOrdinal("Titel");
                        Int32 posGenreNr = rdrFilm.GetOrdinal("GenreNr");
                        Int32 posInVoorraad = rdrFilm.GetOrdinal("InVoorraad");
                        Int32 posUitVoorraad = rdrFilm.GetOrdinal("UitVoorraad");
                        Int32 posPrijs = rdrFilm.GetOrdinal("Prijs");
                        Int32 posTotaalVerhuurd = rdrFilm.GetOrdinal("TotaalVerhuurd");

                        while (rdrFilm.Read())
                        {
                            films.Add(new Film(
                                rdrFilm.GetInt32(posBandNr),
                                rdrFilm.GetString(posTitel),
                                rdrFilm.GetInt32(posGenreNr),
                                rdrFilm.GetInt32(posInVoorraad),
                                rdrFilm.GetInt32(posUitVoorraad),
                                rdrFilm.GetDecimal(posPrijs),
                                rdrFilm.GetInt32(posTotaalVerhuurd)));
                        }
                    }
                }
            }
            return films;
        }
    
        public static List<Genre> GenresOphalen ()
        {
            List<Genre> genres = new List<Genre>();
            using (var convideo = VideotheekDbManager.GetConnection())
            {
                using (var comInfo = convideo.CreateCommand())
                {
                    comInfo.CommandType = CommandType.Text;
                    comInfo.CommandText = "select * from Genres";

                    convideo.Open();
                    using (var rdrGenre = comInfo.ExecuteReader())
                    {
                        Int32 posGenreNr = rdrGenre.GetOrdinal("GenreNr");
                        Int32 posGenre = rdrGenre.GetOrdinal("Genre");

                        while(rdrGenre.Read())
                        {
                            genres.Add(new Genre(rdrGenre.GetInt32(posGenreNr), rdrGenre.GetString(posGenre)));
                        }
                    }
                }
            }
            return genres;
        }
    }
}
