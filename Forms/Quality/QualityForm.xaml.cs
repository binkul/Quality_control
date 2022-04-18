using Quality_Control.Forms.Navigation;
using Quality_Control.Forms.Quality.ModelView;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality
{
    /// <summary>
    /// Logika interakcji dla klasy QualityForm.xaml
    /// </summary>
    public partial class QualityForm : Window
    {
        public QualityForm()
        {
            InitializeComponent();

            QualityMV view = (QualityMV)DataContext;
            QualityDataMV qualityDataMV = Resources["QualityData"] as QualityDataMV;
            NavigationMV navigationMV = Resources["navi"] as NavigationMV;
            qualityDataMV.RefreshQualityData(view.Quality[view.DgRowIndex]);

            navigationMV.ModelView = view;
            qualityDataMV.SetQualityMV(view);

            view.SetQualityDataMV(qualityDataMV);
            view.SetNavigationMV = navigationMV;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
