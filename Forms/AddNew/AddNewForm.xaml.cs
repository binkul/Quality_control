using Quality_Control.Forms.AddNew.ModelView;
using Quality_Control.Forms.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Quality_Control.Forms.AddNew
{
    /// <summary>
    /// Logika interakcji dla klasy AddNewForm.xaml
    /// </summary>
    public partial class AddNewForm : Window
    {
        private readonly AddNewMV view;
        public bool Process { get; private set; } = false;
        public string Product => view.ProductName;
        public string Index => view.ProductIndex;
        public int Number { get; private set; }
        public int Type { get; private set; }
        public long NumberD { get; private set; }
        public DateTime Date => view.ProductionDate;

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
    }
}
