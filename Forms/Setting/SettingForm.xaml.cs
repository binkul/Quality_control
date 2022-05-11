using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace Quality_Control.Forms.Setting
{
    /// <summary>
    /// Logika interakcji dla klasy SettingForm.xaml
    /// </summary>
    public partial class SettingForm : RibbonWindow
    {
        public SettingForm()
        {
            InitializeComponent();
            Height = SystemParameters.PrimaryScreenHeight - 100;
            Width = SystemParameters.PrimaryScreenWidth - 200;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
