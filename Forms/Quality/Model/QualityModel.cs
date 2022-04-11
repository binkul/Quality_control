using System;
using System.ComponentModel;

namespace Quality_Control.Forms.Quality.Model
{
    public class QualityModel : INotifyPropertyChanged
    {
        private long _id;
        private int _number;
        private string _yearNumber;
        private string _productName;
        private string _productIndex = "";
        private long _labBookId = 1;
        private int _productTypeId = 1;
        private string _productTypeName = "";
        private string _remarks = "";
        private string _activeDataFields = "";
        private DateTime _productionDate = DateTime.Now;
        private long _loginId = 1;
        private string _login = "";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public QualityModel(long id, int number, string yearNumber, string productName, string productIndex, long labBookId, int productTypeId, string productTypeName,
            string remarks, string activeDataFields, DateTime productionDate, long loginId, string login)
        {
            _id = id;
            _number = number;
            _yearNumber = yearNumber;
            _productName = productName;
            _productIndex = productIndex;
            _labBookId = labBookId;
            _productTypeId = productTypeId;
            _productTypeName = productTypeName;
            _remarks = remarks;
            _activeDataFields = activeDataFields;
            _productionDate = productionDate;
            _loginId = loginId;
            _login = login;
        }

        public QualityModel(int number, string yearNumber, string productName, string productIndex, long labBookId, int productTypeId, string productTypeName,
            string remarks, string activeDataFields, DateTime productionDate, long loginId, string login)
        {
            _number = number;
            _yearNumber = yearNumber;
            _productName = productName;
            _productIndex = productIndex;
            _labBookId = labBookId;
            _productTypeId = productTypeId;
            _productTypeName = productTypeName;
            _remarks = remarks;
            _activeDataFields = activeDataFields;
            _productionDate = productionDate;
            _loginId = loginId;
            _login = login;
        }

        public long Id
        {
            get => _id;
            set { _id = value; }
        }

        public int Number
        {
            get => _number;
            set { _number = value; }
        }

        public string YearNumber
        {
            get => _yearNumber;
            set { _yearNumber = value; }
        }

        public string ProductName
        {
            get => _productName;
            set { _productName = value; }
        }

        public string ProductIndex
        {
            get => _productIndex;
            set { _productIndex = value; }
        }

        public long LabBookId
        {
            get => _labBookId;
            set { _labBookId = value; }
        }

        public int ProductTypeId
        {
            get => _productTypeId;
            set { _productTypeId = value; }
        }

        public string ProductTypeName
        {
            get => _productTypeName;
            set { _productTypeName = value; }
        }

        public string Remarks
        {
            get => _remarks;
            set 
            { 
                _remarks = value;
                OnPropertyChanged(Remarks);
            }
        }

        public string ActiveDataFields
        {
            get => _activeDataFields;
            set { _activeDataFields = value; }
        }

        public DateTime ProductionDate
        {
            get => _productionDate;
            set { _productionDate = value; }
        }

        public long LoginId
        {
            get => _loginId;
            set { _loginId = value; }
        }

        public string Login
        {
            get => _login;
            set { _login = value; }
        }

        //public int CompareTo(QualityModel other)
        //{
        //    if (other == null)
        //        return 1;

        //    return Number.CompareTo(other.Number);
        //}

    }
}
