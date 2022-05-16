using System.Windows;
using System.Windows.Controls.Ribbon;

namespace Quality_Control.Forms.Statistic
{
    /// <summary>
    /// Logika interakcji dla klasy StatisticRangeForm.xaml
    /// </summary>
    public partial class StatisticRangeForm : RibbonWindow
    {
        public StatisticRangeForm(string title)
        {
            InitializeComponent();
            Height = SystemParameters.PrimaryScreenHeight - 100;
            Width = SystemParameters.PrimaryScreenWidth - 200;
        }
    }
}
