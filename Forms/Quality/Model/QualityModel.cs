using System;
using System.ComponentModel;

namespace Quality_Control.Forms.Quality.Model
{
    public class QualityModel : INotifyPropertyChanged, IComparable<QualityModel>
    {
        private long _id = -1;
        private int _number = 0;
        private string _productName = "";
        private string _productIndex = "";
        private long _labBookId = 1;
        private int _productTypeId = 1;
        private string _productTypeName = "";
        private string _remarks = "";
        private string _activeDataFields = "";
        private DateTime _productionDate = DateTime.Now;
        private long _loginId = 1;
        private string _login = "";
        private bool _modified = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public QualityModel()
        {
        }

        public QualityModel(long id, int number, string productName, string productIndex, long labBookId, int productTypeId, string productTypeName,
            string remarks, string activeDataFields, DateTime productionDate, long loginId, string login)
        {
            _id = id;
            _number = number;
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

        public QualityModel(int number, string productName, string productIndex, long labBookId, int productTypeId, string productTypeName,
            string remarks, string activeDataFields, DateTime productionDate, long loginId, string login)
        {
            _number = number;
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
            set => _id = value;
        }

        public int Number
        {
            get => _number;
            set => _number = value;
        }

        public string YearNumber => ProductionDate.Year.ToString().Substring(2, 2) + "/" + Number.ToString();

        public string ProductName
        {
            get => _productName;
            set => _productName = value;
        }

        public string ProductIndex
        {
            get => _productIndex;
            set => _productIndex = value;
        }

        public long LabBookId
        {
            get => _labBookId;
            set => _labBookId = value;
        }

        public int ProductTypeId
        {
            get => _productTypeId;
            set => _productTypeId = value;
        }

        public string ProductTypeName
        {
            get => _productTypeName;
            set => _productTypeName = value;
        }

        public string Remarks
        {
            get => _remarks;
            set => _remarks = value;
        }

        public string ActiveDataFields
        {
            get => _activeDataFields;
            set => _activeDataFields = value;
        }

        public DateTime ProductionDate
        {
            get => _productionDate;
            set => _productionDate = value;
        }

        public long LoginId
        {
            get => _loginId;
            set => _loginId = value;
        }

        public string Login
        {
            get => _login;
            set => _login = value;
        }

        public bool Modified
        {
            get => _modified;
            set => _modified = value;
        }

        public int CompareTo(QualityModel other)
        {
            return ProductName.CompareTo(other.ProductName);
        }
    }
}
