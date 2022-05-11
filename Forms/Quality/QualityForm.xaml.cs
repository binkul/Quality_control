using Quality_Control.Forms.Navigation;
using Quality_Control.Forms.Quality.ModelView;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality
{
    /// <summary>
    /// Logika interakcji dla klasy QualityForm.xaml
    /// </summary>
    public partial class QualityForm : RibbonWindow
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

        private void DgQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DgQuality.SelectedIndex;
            if (index < 0)
            {
                return;
            }

            object item = DgQuality.Items[index];
            if (!(DgQuality.ItemContainerGenerator.ContainerFromIndex(index) is DataGridRow))
            {
                DgQuality.ScrollIntoView(item);
            }
        }
    }
}
