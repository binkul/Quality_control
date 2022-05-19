using Quality_Control.Forms.Statistic.Model;
using Quality_Control.Forms.Statistic.ModelView;
using Quality_Control.Service;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace Quality_Control.Forms.Statistic
{
    /// <summary>
    /// Logika interakcji dla klasy StatisticRangeForm.xaml
    /// </summary>
    public partial class StatisticRangeForm : RibbonWindow
    {
        public StatisticRangeForm(StatisticDto range)
        {
            InitializeComponent();

            StatisticRangeMV mv = (StatisticRangeMV)DataContext;
            mv.Service = new StatisticService(range);

            Height = SystemParameters.PrimaryScreenHeight - 100;
            Width = SystemParameters.PrimaryScreenWidth - 200;
            Title = range.Title;
            LblTitle.Content = range.Title;
        }
    }
}
