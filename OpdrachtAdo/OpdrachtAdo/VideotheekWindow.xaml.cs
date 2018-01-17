using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AdoGemeenschap;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace OpdrachtAdo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool Aanpassen = false;
        private CollectionViewSource filmViewSource = new CollectionViewSource();
        private CollectionViewSource genreViewSource = new CollectionViewSource();
        private List<Genre> genres = new List<Genre>();
        private List<Film> OudeFilms = new List<Film>();
        private List<Film> GewijzigdeFilms = new List<Film>();
        private List<Film> NieuweFilms = new List<Film>();
        private ObservableCollection<Film> Films = new ObservableCollection<Film>();
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            
            View_Update();
        }
        private void View_Update()
        {
            Films.Clear();
            genres.Clear();

            Films = filmDbManager.FilmsOphalen();
            filmViewSource = ((CollectionViewSource)(this.FindResource("filmViewSource")));
            filmViewSource.Source = Films;
            Films.CollectionChanged += On_Collection_Changed;
            genres = filmDbManager.GenresOphalen();
            genres.Insert(0, new Genre());
            genreViewSource = ((CollectionViewSource)(this.FindResource("genreViewSource")));
            genreViewSource.Source = genres;
            genreComboBox.SelectedIndex = ((Film)filmViewSource.View.CurrentItem).GenreNr;
        }

        private void buttonToevoegen_Click(object sender, RoutedEventArgs e)
        {
            if (Aanpassen)
            {
                Films[Films.Count - 1].Changed = false;
                this.ToggleStatus();
            }
           else
            {
                Films.Add(new Film());
                filmViewSource.View.MoveCurrentToLast();
                this.ToggleStatus();
            }

        }
        private void buttonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            if (Aanpassen)
            {
                Films.RemoveAt(Films.Count - 1);
                this.ToggleStatus();
                filmViewSource.View.MoveCurrentToFirst();
                View_Update();
            }
            else
            {
                if (MessageBox.Show("Ben je zeker dat je deze film wil verwijderen ?", "Verwijderen", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    Films.RemoveAt(filmViewSource.View.CurrentPosition);
                }
                filmViewSource.View.MoveCurrentToFirst();
                View_Update();
            }


        }
        private void buttonAllesOpslaan_Click(object sender, RoutedEventArgs e)
        {
            foreach(Film film in Films)
            {
                if(film.Changed)
                {
                    GewijzigdeFilms.Add(film);
                    film.Changed = false;
                }
            }
            if (MessageBox.Show("Wilt u alles wegschrijven naar de database ?","Opslaan",MessageBoxButton.YesNo,MessageBoxImage.Question,MessageBoxResult.Yes)==MessageBoxResult.Yes)
            {
                try
                {
                    var manager = new filmDbManager();
                    manager.NieuweFilmsOpslaan(NieuweFilms);
                    manager.OudeFilmsVerwijderen(OudeFilms);
                    manager.WijzigingenDoorvoeren(GewijzigdeFilms);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                View_Update();
            }
        }

        
        private void ToggleStatus()
        {
            filmListBox.IsEnabled = !filmListBox.IsEnabled;
            buttonAllesOpslaan.IsEnabled = !buttonAllesOpslaan.IsEnabled;
            ButtonVerhuur.IsEnabled = !ButtonVerhuur.IsEnabled;
            ButtonTerug.IsEnabled = !ButtonTerug.IsEnabled;
            gridFilm.IsEnabled = !gridFilm.IsEnabled;
            Aanpassen = !Aanpassen;
            if (Aanpassen)
            {
                buttonToevoegen.Content = "Bevestigen";
                buttonVerwijderen.Content = "Annuleren";
                ClearInvoer();
            }
            else
            {
                buttonToevoegen.Content = "Toevoegen";
                buttonVerwijderen.Content = "Verwijderen";
                filmViewSource.View.MoveCurrentToFirst();
            }
        }
        private void ClearInvoer()
        {
            titelTextBox.Text = string.Empty;
            genreComboBox.SelectedIndex = 0;
            inVoorraadTextBox.Text = string.Empty;
            uitVoorraadTextBox.Text = string.Empty;
            prijsTextBox.Text = string.Empty;
            totaalVerhuurdTextBox.Text = string.Empty;
        }
        private bool FoutAanwezig()
        {
            bool foutgevonden = false;
            foreach(var child in gridFilm.Children)
            {
                if(child is TextBox)
                {
                    if(Validation.GetHasError((TextBox)child))
                    {
                        foutgevonden = true;
                    }
                }
                if(child is ComboBox)
                {
                    if(Validation.GetHasError((ComboBox)child))
                    {
                        foutgevonden = true;
                    }
                }
            }
            return foutgevonden;
        }
        private void On_Collection_Changed(object sender,NotifyCollectionChangedEventArgs e)
        {
            if(e.OldItems!=null)
            {
                foreach(Film film in e.OldItems)
                {
                    OudeFilms.Add(film);
                }
            }
            if(e.NewItems!=null)
            {
                foreach(Film film in e.NewItems)
                {
                    NieuweFilms.Add(film);
                }
            }
        }

        private void buttonToevoegen_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (FoutAanwezig())
                e.Handled = true;
        }
        private void ButtonVerhuur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Film)filmViewSource.View.CurrentItem).verhuren();
                filmViewSource.View.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"Verhuur",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
        }
        private void ButtonTerug_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Film)filmViewSource.View.CurrentItem).TerugBrengen();
                filmViewSource.View.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Verhuur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
