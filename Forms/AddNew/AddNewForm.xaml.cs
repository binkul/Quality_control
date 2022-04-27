using Quality_Control.Forms.AddNew.ModelView;
using Quality_Control.Forms.Navigation;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.AddNew
{
    /// <summary>
    /// Logika interakcji dla klasy AddNewForm.xaml
    /// </summary>
    public partial class AddNewForm : Window
    {
        private readonly AddNewMV view;
        public bool Cancel { get; private set; } = true;
        public string Product => view.ProductName;
        public string Index => view.ProductIndex;
        public int Number => Convert.ToInt32(view.ProductNumber);
        public int Type => view.ProductType;
        public long LabBookId => view.ProductLabBookId;
        public DateTime ProductionDate => view.ProductionDate;

        public AddNewForm()
        {
            InitializeComponent();

            view = (AddNewMV)DataContext;
            NavigationMV navigationMV = Resources["navi"] as NavigationMV;

            navigationMV.ModelView = view;
            view.SetNavigationMV = navigationMV;

            Height = SystemParameters.PrimaryScreenHeight - 100;
            if (SystemParameters.PrimaryScreenWidth <= 800)
                Width = 600;
            else if (SystemParameters.PrimaryScreenWidth > 800 && SystemParameters.PrimaryScreenWidth <= 1000)
                Width = 800;
            else
                Width = 1000;
        }

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancel = true;
            Close();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if (view.IsNumberCorrect())
            {
                Cancel = false;
                Close();
            }
            else
            {
                _ = MessageBox.Show("Podany numer produckji P" + Number + " z dnia produkcji " + ProductionDate.ToShortDateString() +
                    " istnieje już w bazie danych!", "Błędny numer", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DgProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DgProduct.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            object item = DgProduct.Items[index];
            if (!(DgProduct.ItemContainerGenerator.ContainerFromIndex(index) is DataGridRow))
            {
                DgProduct.ScrollIntoView(item);
            }
        }
    }
}
