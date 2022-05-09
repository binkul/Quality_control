using System;
using System.Windows;

namespace Quality_Control.Forms.InputBox
{
    /// <summary>
    /// Logika interakcji dla klasy InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public InputBox(string question, string defaultAnswer = "")
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string Answer => txtAnswer.Text;

    }
}
