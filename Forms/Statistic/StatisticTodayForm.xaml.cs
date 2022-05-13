using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace Quality_Control.Forms.Statistic
{
    /// <summary>
    /// Logika interakcji dla klasy StatisticTodayForm.xaml
    /// </summary>
    public partial class StatisticTodayForm : RibbonWindow
    {
        public StatisticTodayForm()
        {
            InitializeComponent();
            Title = "Wyniki na dzień - " + DateTime.Now.ToShortDateString();
            Height = SystemParameters.PrimaryScreenHeight - 100;
            Width = SystemParameters.PrimaryScreenWidth - 200;
        }
    }
}
