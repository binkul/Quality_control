using Quality_Control.Forms.Navigation;
using System;
using System.ComponentModel;

namespace Quality_Control.Forms.AddNew.ModelView
{
    public class AddNewMV : INotifyPropertyChanged, INavigation
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Navigation

        public int GetRowCount => throw new NotImplementedException();

        public int DgRowIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
