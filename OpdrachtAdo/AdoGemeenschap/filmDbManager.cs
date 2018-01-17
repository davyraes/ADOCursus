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
        public List<Film> NieuweFilmsOpslaan(List<Film> films)
        {
            List<Film> NietOpgeslagen = new List<Film>();
            using (var conVideo = VideotheekDbManager.GetConnection())
            {
                using (var comSave = conVideo.CreateCommand())
                {
                    comSave.CommandType = CommandType.StoredProcedure;
                    comSave.CommandText = "FilmOpslaan";

                    var parTitel = comSave.CreateParameter();
                    parTitel.ParameterName = "@Titel";
                    comSave.Parameters.Add(parTitel);

                    var parGenreNr = comSave.CreateParameter();
                    parGenreNr.ParameterName = "@GenreNr";
                    comSave.Parameters.Add(parGenreNr); 
                    
                    var parInVoorraad = comSave.CreateParameter();
                    parInVoorraad.ParameterName = "@InVoorraad";
                    comSave.Parameters.Add(parInVoorraad);

                    var parUitVoorraad = comSave.CreateParameter();
                    parUitVoorraad.ParameterName = "@UitVoorraad";
                    comSave.Parameters.Add(parUitVoorraad);

                    var parPrijs = comSave.CreateParameter();
                    parPrijs.ParameterName = "@Prijs";
                    comSave.Parameters.Add(parPrijs);

                    var parTotaalVerhuurd = comSave.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@TotaalVerhuurd";
                    comSave.Parameters.Add(parTotaalVerhuurd);

                    conVideo.Open();
                    foreach(Film film in films)
                    {
                        try
                        {
                            parTitel.Value = film.Titel;
                            parGenreNr.Value = film.GenreNr;
                            parInVoorraad.Value = film.InVoorraad;
                            parUitVoorraad.Value = film.UitVoorraad;
                            parPrijs.Value = film.Prijs;
                            parTotaalVerhuurd.Value = film.TotaalVerhuurd;
                            if (comSave.ExecuteNonQuery() == 0)
                            {
                                NietOpgeslagen.Add(film);
                            }
                        }
                        catch(Exception)
                        {
                            NietOpgeslagen.Add(film);
                        }
                    }

                }
            }
            return NietOpgeslagen;
        }
        public List<Film> OudeFilmsVerwijderen(List<Film> films)
        {
            List<Film> nietVerwijderd = new List<Film>();
            using (var conVideo = VideotheekDbManager.GetConnection())
            {
                using (var comDelete = conVideo.CreateCommand())
                {
                    comDelete.CommandType = CommandType.Text;
                    comDelete.CommandText = "Delete from films where BandNr=@BandNr";

                    var parBandNr = comDelete.CreateParameter();
                    parBandNr.ParameterName = "@BandNr";
                    comDelete.Parameters.Add(parBandNr);

                    conVideo.Open();
                    foreach(Film film in films)
                    {
                        try
                        {
                            parBandNr.Value = film.BandNr;
                            if(comDelete.ExecuteNonQuery()==0)
                            {
                                nietVerwijderd.Add(film);
                            }
                        }
                        catch(Exception)
                        {
                            nietVerwijderd.Add(film);
                        }
                    }
                }
            }
            return nietVerwijderd;
        }
        public List<Film> WijzigingenDoorvoeren(List<Film>films)
        {
            List<Film> nietDoorgevoerd = new List<Film>();
            using (var convideo = VideotheekDbManager.GetConnection())
            {
                using (var comEdit = convideo.CreateCommand())
                {
                    comEdit.CommandType = CommandType.Text;
                    comEdit.CommandText = "update films set InVoorraad=@InVoorraad,UitVoorraad=@uitVoorraad,TotaalVerhuurd=@TotaalVerhuurd where BandNr=@Bandnr";

                    var parInVoorraad = comEdit.CreateParameter();
                    parInVoorraad.ParameterName = "@InVoorraad";
                    comEdit.Parameters.Add(parInVoorraad);

                    var parUitVoorraad = comEdit.CreateParameter();
                    parUitVoorraad.ParameterName = "@UitVoorraad";
                    comEdit.Parameters.Add(parUitVoorraad);

                    var parTotaalVerhuurd = comEdit.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@TotaalVerhuurd";
                    comEdit.Parameters.Add(parTotaalVerhuurd);

                    var parBandNr = comEdit.CreateParameter();
                    parBandNr.ParameterName = "@BandNr";
                    comEdit.Parameters.Add(parBandNr);

                    convideo.Open();
                    foreach(Film film in films)
                    {
                        try
                        {
                            parInVoorraad.Value = film.InVoorraad;
                            parUitVoorraad.Value = film.UitVoorraad;
                            parTotaalVerhuurd.Value = film.TotaalVerhuurd;
                            parBandNr.Value = film.BandNr;
                            if(comEdit.ExecuteNonQuery()==0)
                            {
                                nietDoorgevoerd.Add(film);
                            }
                        }catch(Exception)
                        {
                            nietDoorgevoerd.Add(film);
                        }
                    }
                }
            }
            return nietDoorgevoerd;
        }
    }
}
