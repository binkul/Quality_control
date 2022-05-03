using Quality_Control.Forms.Modification.ModelView;
using Quality_Control.Forms.Quality.Model;
using System.Windows;

namespace Quality_Control.Forms.Modification
{
    /// <summary>
    /// Logika interakcji dla klasy ModificationForm.xaml
    /// </summary>
    public partial class ModificationForm : Window
    {
        private QualityModel _quality;
        public bool Cancel { get; private set; } = true;
        public string Fields { get; private set; } = "";

        public ModificationForm(QualityModel quality)
        {
            _quality = quality;
            Fields = _quality.ActiveDataFields;
            InitializeComponent();
            Height = SystemParameters.PrimaryScreenHeight - 100;
            ProductNameLbl.Content = "P" + quality.Number + " " + quality.ProductName;

            ModificationMV view = (ModificationMV)DataContext;
            view.Quality = _quality;
            view.InitiateFields();
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            ModificationMV view = (ModificationMV)DataContext;
            Fields = view.SettingFields;
            Cancel = false;
            Close();
        }
    }
}
