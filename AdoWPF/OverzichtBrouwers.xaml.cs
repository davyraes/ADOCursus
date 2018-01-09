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
using System.Windows.Shapes;
using AdoGemeenschap;

namespace AdoWPF
{
    /// <summary>
    /// Interaction logic for OverzichtBrouwers.xaml
    /// </summary>
    public partial class OverzichtBrouwers : Window
    {
        public OverzichtBrouwers()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulDeGrid();
            textBoxZoeken.Focus();
            
        }
        private void VulDeGrid()
        {
            CollectionViewSource brouwerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("brouwerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // brouwerViewSource.Source = [generic data source]
            var manager = new BrouwerManager();
            List<Brouwer> brouwersOb = new List<Brouwer>();
            brouwersOb = manager.GetBrouwersBeginNaam(textBoxZoeken.Text);
            brouwerViewSource.Source = brouwersOb;

        }

        private void buttonZoeken_Click(object sender, RoutedEventArgs e)
        {
            VulDeGrid();
        }
        private void textBoxZoeken_KeyUp(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                VulDeGrid();
        }
    }
}
