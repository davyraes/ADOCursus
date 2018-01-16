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
        private ObservableCollection<Film> Films = new ObservableCollection<Film>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Films = filmDbManager.FilmsOphalen();
            genres = filmDbManager.GenresOphalen();
            genres.Insert(0, new Genre(0, string.Empty));
            filmViewSource = ((CollectionViewSource)(this.FindResource("filmViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // filmViewSource.Source = [generic data source]
            filmViewSource.Source = Films;
            genreViewSource = ((CollectionViewSource)(this.FindResource("genreViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // genreViewSource.Source = [generic data source]
            genreViewSource.Source = genres;
        }

        private void buttonToevoegen_Click(object sender, RoutedEventArgs e)
        {
            Films.Add(new Film());
            this.ToggleStatus();
           

        }
        private void buttonVerwijderen_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleStatus();

        }
        private void buttonAllesOpslaan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonVerhuur_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ToggleStatus()
        {            
            filmListBox.IsEnabled = !filmListBox.IsEnabled;
            buttonAllesOpslaan.IsEnabled = !buttonAllesOpslaan.IsEnabled;
            ButtonVerhuur.IsEnabled = !ButtonVerhuur.IsEnabled;
            gridFilm.IsEnabled = !gridFilm.IsEnabled;
            Aanpassen = !Aanpassen;
            if (Aanpassen)
            {
                buttonToevoegen.Content = "Bevestigen";
                buttonVerwijderen.Content = "Annuleren";
                filmViewSource.View.MoveCurrentToLast();
            }
            else
            {
                buttonToevoegen.Content = "Toevoegen";
                buttonVerwijderen.Content = "Verwijderen";
            }
        }
    }
}
