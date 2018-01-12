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
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace AdoWPF
{
    /// <summary>
    /// Interaction logic for OverzichtBrouwers.xaml
    /// </summary>
    public partial class OverzichtBrouwers : Window
    {
        private CollectionViewSource brouwerViewSource;
        public ObservableCollection<Brouwer> brouwersOb= new ObservableCollection<Brouwer>();
        public OverzichtBrouwers()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            VulDeGrid();
            textBoxZoeken.Focus();
            var nummers = (from b in brouwersOb orderby b.Postcode select b.Postcode.ToString()).Distinct().ToList();
            nummers.Insert(0, "alles");
            comboBoxPostCode.ItemsSource = nummers;
            comboBoxPostCode.SelectedIndex = 0;
        }
        private void VulDeGrid()
        {
            brouwerViewSource = ((CollectionViewSource)(this.FindResource("brouwerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // brouwerViewSource.Source = [generic data source]
            var manager = new BrouwerManager();
            brouwersOb = manager.GetBrouwersBeginNaam(textBoxZoeken.Text);
            brouwerViewSource.Source = brouwersOb;
            labelTotalRowCount.Content = brouwerDataGrid.Items.Count;
            brouwersOb.CollectionChanged += this.OncollectionChanged;
            GoUpdate();
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

        private void GoToNextButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToNext();
            GoUpdate();
        }

        private void GoToPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToPrevious();
            GoUpdate();
        }

        private void GoToLastButton_Click(object sender, RoutedEventArgs e)
        {
            brouwerViewSource.View.MoveCurrentToLast();
            GoUpdate();
        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            int position;
            int.TryParse(textBoxGo.Text, out position);
            if (position > 0 && position <= brouwerDataGrid.Items.Count)
                brouwerViewSource.View.MoveCurrentToPosition(position - 1);
            else
                MessageBox.Show("the input is not valid");
            GoUpdate();
        }

        private void brouwerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GoUpdate();
        }
        private void GoUpdate()
        {
            GoToFirstButton.IsEnabled = brouwerViewSource.View.CurrentPosition != 0;
            GoToPreviousButton.IsEnabled = brouwerViewSource.View.CurrentPosition != 0;
            GoToNextButton.IsEnabled = brouwerViewSource.View.CurrentPosition != brouwerDataGrid.Items.Count - 2;
            GoToLastButton.IsEnabled = brouwerViewSource.View.CurrentPosition != brouwerDataGrid.Items.Count - 2;
            if (brouwerDataGrid.Items.Count != 0)
                if (brouwerDataGrid.SelectedItem != null)
                {
                    brouwerDataGrid.ScrollIntoView(brouwerDataGrid.SelectedItem);
                    listBoxBrouwers.ScrollIntoView(listBoxBrouwers.SelectedItem);
                }
            textBoxGo.Text = (brouwerViewSource.View.CurrentPosition + 1).ToString();
        }

        private void checkBoxPostcode0_Click(object sender, RoutedEventArgs e)
        {
            Binding binding1 = BindingOperations.GetBinding(postcodeTextBox, TextBox.TextProperty);
            binding1.ValidationRules.Clear();
            var binding2 = (postcodeColumn as DataGridBoundColumn).Binding as Binding;
            binding2.ValidationRules.Clear();

            brouwerDataGrid.RowValidationRules.Clear();
            switch(checkBoxPostcode0.IsChecked)
            {
                case true:
                    binding1.ValidationRules.Add(new PostcodeRangeRule0());
                    binding2.ValidationRules.Add(new PostcodeRangeRule0());
                    brouwerDataGrid.RowValidationRules.Add(new PostcodeRangeRule0());
                    break;
                case false:
                    binding1.ValidationRules.Add(new PostcodeRangeRule());
                    binding2.ValidationRules.Add(new PostcodeRangeRule());
                    brouwerDataGrid.RowValidationRules.Add(new PostcodeRangeRule());
                    break;
                default:
                    binding1.ValidationRules.Add(new PostcodeRangeRule());
                    binding2.ValidationRules.Add(new PostcodeRangeRule());
                    brouwerDataGrid.RowValidationRules.Add(new PostcodeRangeRule());
                    break;
            }
            //binding1.ValidationRules[0].ValidatesOnTargetUpdated = true;
            //binding1.ValidationRules[0].ValidationStep = ValidationStep.UpdatedValue;
            //binding2.ValidationRules[0].ValidatesOnTargetUpdated = true;
            //binding2.ValidationRules[0].ValidationStep = ValidationStep.UpdatedValue;
            //brouwerDataGrid.RowValidationRules[0].ValidatesOnTargetUpdated = true;
            //brouwerDataGrid.RowValidationRules[0].ValidationStep = ValidationStep.UpdatedValue;

        }

        private void brouwerDataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
            if (FoutVinden())
                e.Handled = true;
        }

        private bool FoutVinden()
        {
            bool foutGevonden = false;
            foreach (var c in gridDetail.Children)
            {
                if (c is AdornerDecorator)
                {
                    if (Validation.GetHasError(((AdornerDecorator)c).Child))
                    {
                        foutGevonden = true;
                    }
                }
                else if (Validation.GetHasError((DependencyObject)c))
                {
                    foutGevonden = true;
                }
            }
            foreach (var c in brouwerDataGrid.ItemsSource)
            {
                var d = brouwerDataGrid.ItemContainerGenerator.ContainerFromItem(c);
                if (d is DataGridRow)
                {
                    if (Validation.GetHasError(d))
                        foutGevonden = true;
                }
            }
            return foutGevonden;
        }

        private void brouwerDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (FoutVinden())
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                    e.Handled = true;
        }

        private void comboBoxPostCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxPostCode.SelectedIndex == 0)
                brouwerDataGrid.Items.Filter = null;
            else
                brouwerDataGrid.Items.Filter = new Predicate<object>(PostCodeFilter);
            GoUpdate();
            labelTotalRowCount.Content = brouwerDataGrid.Items.Count-1;
        }
        public bool PostCodeFilter(object br)
        {
            Brouwer b = br as Brouwer;
            return(b.Postcode==Convert.ToInt16(comboBoxPostCode.SelectedValue));
        }
        public List<Brouwer> OudeBrouwers = new List<Brouwer>();
        public List<Brouwer> nieuweBrouwers = new List<Brouwer>();
        public List<Brouwer> GewijzigdeBrouwers = new List<Brouwer>();
        void OncollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems!=null)
            {
                foreach (Brouwer oudebrouwer in e.OldItems)
                    OudeBrouwers.Add(oudebrouwer);
            }
            if (e.NewItems != null)
            {
                foreach (Brouwer newBrouwer in e.NewItems)
                    nieuweBrouwers.Add(newBrouwer);
            }
            labelTotalRowCount.Content = brouwerDataGrid.Items.Count;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            brouwerDataGrid.CommitEdit(DataGridEditingUnit.Row, true);

            List<Brouwer> resultaatBrouwers = new List<Brouwer>();
            var manager = new BrouwerManager();
            if (OudeBrouwers.Count()!=0)
            {
                resultaatBrouwers = manager.SchrijfVerwijderingen(OudeBrouwers);
                if (resultaatBrouwers.Count>0)
                {
                    StringBuilder boodschap = new StringBuilder();
                    boodschap.Append("niet verwijderd: \n");
                    foreach(var b in resultaatBrouwers)
                    {
                        boodschap.Append("nummer: " + b.BrouwersNr + " : " + b.BrNaam + " niet\n");
                    }
                    MessageBox.Show(boodschap.ToString());
                }
            }
            MessageBox.Show(OudeBrouwers.Count - resultaatBrouwers.Count + " brouwer(s) verwijderd in de database", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            resultaatBrouwers.Clear();

            if (nieuweBrouwers.Count()!=0)
            {
                resultaatBrouwers = manager.SchrijfToevoegingen(nieuweBrouwers);
                if(resultaatBrouwers.Count>0)
                {
                    StringBuilder boodschap = new StringBuilder();
                    boodschap.Append("niet Toegevoegd: \n");
                    foreach (var b in resultaatBrouwers)
                    {
                        boodschap.Append("nummer: " + b.BrouwersNr + " : " + b.BrNaam + " niet\n");
                    }
                    MessageBox.Show(boodschap.ToString());
                }
            }
            MessageBox.Show(nieuweBrouwers.Count - resultaatBrouwers.Count + " brouwer(s) toegevoegd in de database", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            foreach (Brouwer b in brouwersOb)
            {
                if ((b.changed == true) && (b.BrouwersNr != 0))
                {
                    GewijzigdeBrouwers.Add(b);
                    b.changed = false;
                }
            }
            resultaatBrouwers.Clear();
            if (nieuweBrouwers.Count() != 0)
            {
                resultaatBrouwers = manager.SchrijfWijzigingen(GewijzigdeBrouwers);
                if (resultaatBrouwers.Count > 0)
                {
                    StringBuilder boodschap = new StringBuilder();
                    boodschap.Append("niet Gewijzigd: \n");
                    foreach (var b in resultaatBrouwers)
                    {
                        boodschap.Append("nummer: " + b.BrouwersNr + " : " + b.BrNaam + " niet\n");
                    }
                    MessageBox.Show(boodschap.ToString());
                }
            }
            MessageBox.Show(GewijzigdeBrouwers.Count - resultaatBrouwers.Count + " brouwer(s) toegevoegd in de database", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            VulDeGrid();
            OudeBrouwers.Clear();
            nieuweBrouwers.Clear();
            GewijzigdeBrouwers.Clear();                
        }
        
    }
}
