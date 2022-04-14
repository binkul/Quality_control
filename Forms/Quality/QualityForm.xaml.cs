using Quality_Control.Forms.Quality.ModelView;
using System.Windows;

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
            qualityDataMV.RefreshQualityData(view.Quality[view.SelectedIndex]);

            view.SetQualityDataMV(qualityDataMV);
        }
    }
}
