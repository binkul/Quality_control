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

namespace Quality_Control.Forms.AddNew
{
    /// <summary>
    /// Logika interakcji dla klasy AddNewForm.xaml
    /// </summary>
    public partial class AddNewForm : Window
    {
        public AddNewForm()
        {
            InitializeComponent();
            Height = SystemParameters.PrimaryScreenHeight - 100;
            if (SystemParameters.PrimaryScreenWidth <= 800)
                Width = 600;
            else if (SystemParameters.PrimaryScreenWidth > 800 && SystemParameters.PrimaryScreenWidth <= 1000)
                Width = 800;
            else
                Width = 1000;
        }
    }
}
