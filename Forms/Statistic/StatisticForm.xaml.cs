using Quality_Control.Forms.Navigation;
using Quality_Control.Forms.Statistic.ModelView;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;

namespace Quality_Control.Forms.Statistic
{
    /// <summary>
    /// Logika interakcji dla klasy Statistic.xaml
    /// </summary>
    public partial class StatisticForm : RibbonWindow
    {
        private readonly StatisticMV view;

        public StatisticForm()
        {
            InitializeComponent();

            view = (StatisticMV)DataContext;
            NavigationMV navigationMV = Resources["navi"] as NavigationMV;

            navigationMV.ModelView = view;
            view.SetNavigationMV = navigationMV;

            Height = SystemParameters.PrimaryScreenHeight - 100;
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
