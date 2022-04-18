using GalaSoft.MvvmLight.Command;
using Quality_Control.Forms.Quality.Command;
using Quality_Control.Forms.Quality.Model;
using Quality_Control.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityDataMV : INotifyPropertyChanged
    {
        private ICommand _delQualityData;

        private QualityMV _qualityMV;
        private readonly QualityDataService _service = new QualityDataService();
        private readonly DataTable _qualityDataTable;
        private DataRowView _actualDataGridRow;
        private int _dataGridRowIndex = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        public bool Modified { get; set; } = false;
        public DataView QualityDataView { get; }
        public RelayCommand<InitializingNewItemEventArgs> OnInitializingNewQualityDataCommand { get; set; }

        public QualityDataMV()
        {
            OnInitializingNewQualityDataCommand = new RelayCommand<InitializingNewItemEventArgs>(OnInitializingNewQualityDataCommandExecuted);
            _qualityDataTable = _service.GetQualityDataById(-1);
            _qualityDataTable.ColumnChanged += QualityDataTable_ColumnChanged;
            QualityDataView = new DataView(_qualityDataTable);
        }

        public int DataGriddRowIndex
        {
            get => _dataGridRowIndex;
            set => _dataGridRowIndex = value;
        }

        public DataRowView ActualDataGridRow
        {
            get => _actualDataGridRow;
            set => _actualDataGridRow = value;
        }

        private void QualityDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            Modified = true;
        }

        protected void OnPropertyChanged(params string[] names)
        {
            if (PropertyChanged != null)
            {
                foreach (string name in names)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public List<string> GetActiveFields { get; } = new List<string>() { "measure_date", "temp", "density", "pH", "vis_1", "vis_5", "vis_20", "disc", "comments" };

        public void SetQualityMV(QualityMV qualityMV) => _qualityMV = qualityMV;

        public void RefreshQualityData(QualityModel quality)
        {
            Save();
            if (quality != null)
            {
                _service.RefreshQualityData(quality.Id, _qualityDataTable);
                GetActiveFields.Clear();
                GetActiveFields.AddRange(_service.GetActiveFields(quality));
            }
            else
            {
                _service.RefreshQualityData(-1, _qualityDataTable);
            }
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

            Modified = false;
        }

        public void DeleteQualityData()
        {
            if (ActualDataGridRow == null || ActualDataGridRow.IsNew) return;

            if (MessageBox.Show("Czy usunąć zaznaczony rekord?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                long id = Convert.ToInt64(ActualDataGridRow.Row["id"]);
                _service.Delete(id);
            }

            if (DataGriddRowIndex < QualityDataView.Count)
                QualityDataView.Delete(DataGriddRowIndex);
        }
    }
}
