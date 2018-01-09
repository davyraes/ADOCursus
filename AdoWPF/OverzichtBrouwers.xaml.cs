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
        private CollectionViewSource brouwerViewSource;
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
            brouwerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("brouwerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // brouwerViewSource.Source = [generic data source]
            var manager = new BrouwerManager();
            List<Brouwer> brouwersOb = new List<Brouwer>();
            brouwersOb = manager.GetBrouwersBeginNaam(textBoxZoeken.Text);
            brouwerViewSource.Source = brouwersOb;
            GoUpdate();
            labelTotalRowCount.Content = brouwerDataGrid.Items.Count;
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

        private void GoToFirstButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToFirst();
            GoUpdate();
        }

        private void GoToPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToPrevious();
            GoUpdate();
        }

        private void GoToNextButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToNext();
            GoUpdate();
        }

        private void GoToLastButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToLast();
            GoUpdate();
        }
        private void GoUpdate()
        {
            GoToFirstButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == 0);
            GoToPreviousButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == 0);
            GoToNextButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == brouwerDataGrid.Items.Count - 1);
            GoToLastButton.IsEnabled = !(brouwerViewSource.View.CurrentPosition == brouwerDataGrid.Items.Count - 1);
            if (brouwerDataGrid.Items.Count != 0)
                if (brouwerDataGrid.SelectedItem != null)
                    brouwerDataGrid.ScrollIntoView(brouwerDataGrid.SelectedItem);
            textBoxGo.Text = (brouwerViewSource.View.CurrentPosition + 1).ToString();
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            int position;
            int.TryParse(textBoxGo.Text, out position);
            if (position > 0 && position < brouwerDataGrid.Items.Count)
                brouwerViewSource.View.MoveCurrentToPosition(position - 1);
            else
                MessageBox.Show("the input index is not valid");
            GoUpdate();
        }

        private void brouwerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GoUpdate();
        }
    }
}
