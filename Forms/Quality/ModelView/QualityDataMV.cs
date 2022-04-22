using GalaSoft.MvvmLight.Command;
using Quality_Control.Forms.Quality.Command;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityDataMV : INotifyPropertyChanged
    {
        private ICommand _delQualityData;

        private QualityMV _qualityMV;
        private readonly QualityDataService _service = new QualityDataService();
        private DataRowView _actualDataGridRow;
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<InitializingNewItemEventArgs> OnInitializingNewQualityDataCommand { get; set; }

        public QualityDataMV()
        {
            OnInitializingNewQualityDataCommand = new RelayCommand<InitializingNewItemEventArgs>(OnInitializingNewQualityDataCommandExecuted);
        }

        public int DataGriddRowIndex
        {
            get => _service.DataGridRowIndex;
            set => _service.DataGridRowIndex = value;
        }

        public bool Modified
        {
            get => _service.Modified;
            set => _service.Modified = value;
        }

        public DataView QualityDataView
        {
            get => _service.QualityDataView;
        }

        public DataRowView ActualDataGridRow
        {
            get => _actualDataGridRow;
            set => _actualDataGridRow = value;
        }

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<string> GetActiveFields
        {
            get => _service.ActiveFields;
        }

        public void SetQualityMV(QualityMV qualityMV) => _qualityMV = qualityMV;

        public void RefreshQualityData(QualityModel quality)
        {
            _service.RefreshQualityData(quality);
            OnPropertyChanged(nameof(GetActiveFields));
        }

        public void OnInitializingNewQualityDataCommandExecuted(InitializingNewItemEventArgs e)
        {
            QualityModel quality = _qualityMV.GetCurrentQuality;

            DataRowView row = e.NewItem as DataRowView;
            row["id"] = -1;
            row["quality_id"] = quality.Id;
            row["measure_date"] = DateTime.Today;
            Modified = true;
        }

        public ICommand DeleteQualityDataButton
        {
            get
            {
                if (_delQualityData == null) _delQualityData = new DeleteQualityDataButton(this);
                return _delQualityData;
            }
        }

        public void Save()
        {
            _ = _service.Save(_qualityMV.ActualQuality.Id);
        }

        public void Delete()
        {
            if (ActualDataGridRow == null || ActualDataGridRow.IsNew)
                return;
            else
                _service.Delete(Convert.ToInt64(ActualDataGridRow.Row["id"]));



            //if (MessageBox.Show("Czy usunąć zaznaczony rekord?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    long id = Convert.ToInt64(ActualDataGridRow.Row["id"]);
            //    _service.Delete(id);
            //}

            //if (DataGriddRowIndex < QualityDataView.Count)
            //    QualityDataView.Delete(DataGriddRowIndex);
        }
    }
}
