using System;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

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

        private void DgQualityData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Right)
            {
                // Cancel [Enter] key event.
                e.Handled = true;
                // Press [Tab] key programatically.
                KeyEventArgs tabKeyEvent = new KeyEventArgs(
                  e.KeyboardDevice, e.InputSource, e.Timestamp, Key.Tab)
                {
                    RoutedEvent = Keyboard.KeyDownEvent
                };
                _ = InputManager.Current.ProcessInput(tabKeyEvent);
            }
        }
    }
}
