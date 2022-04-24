using Quality_Control.Forms.Quality.Model;
using Quality_Control.Repository;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Quality_Control.Service
{
    public class QualityDataService
    {
        public static readonly List<string> DefaultFields = new List<string>() { "measure_date", "temp", "density", "pH", "vis_1", "vis_5", "vis_20", "disc", "comments" };
        private readonly QualityDataRepository _repository;
        private readonly DataTable _qualityDataTable;
        private bool _modified = false;
        private readonly DataView _qualityDataView;
        private int _dataGridRowIndex = 0;

        public QualityDataService()
        {
            _repository = new QualityDataRepository();
            _qualityDataTable = GetQualityDataById(-1);
            _qualityDataTable.ColumnChanged += QualityDataTable_ColumnChanged;
            _qualityDataView = new DataView(_qualityDataTable);

        }

        public bool Modified
        {
            get => _modified;
            set => _modified = value;
        }

        public DataView QualityDataView
        {
            get => _qualityDataView;
        }

        public int DataGridRowIndex
        {
            get => _dataGridRowIndex;
            set => _dataGridRowIndex = value;
        }

        public DataTable GetQualityDataById(long id)
        {
            return _repository.GetQualityDataByQualityId(id);
        }

        private void QualityDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            _modified = true;
        }

        public List<string> ActiveFields { get; } = new List<string>(DefaultFields);

        public List<string> GetActiveFields(QualityModel quality)
        {
            List<string> result = new List<string>();
            string fields = quality.ActiveDataFields;

            if (!string.IsNullOrEmpty(fields))
            {
                string[] tmp = fields.Split('|');
                result.AddRange(tmp);
            }
            else
                result = DefaultFields;

            return result;
        }

        public void RefreshQualityData(long id, DataTable table)
        {
            table.Rows.Clear();
            _repository.LoadQualityDataById(table, id);
        }

        public void RefreshQualityData(QualityModel quality)
        {
            if (quality == null) return;

            Save(quality.Id);
            if (quality != null)
            {
                RefreshQualityData(quality.Id, _qualityDataTable);
                ActiveFields.Clear();
                ActiveFields.AddRange(GetActiveFields(quality));
            }
            else
            {
                RefreshQualityData(-1, _qualityDataTable);
            }
        }

        public void Delete(long id)
        {
            if (MessageBox.Show("Czy usunąć zaznaczony rekord?", "Usuwanie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _ = _repository.DeleteQualityDataById(id);
                if (DataGridRowIndex < QualityDataView.Count)
                    QualityDataView.Delete(DataGridRowIndex);
            }
        }

        public bool Save(long id)
        {
            bool resultAddedData = SaveNewQualityData();
            bool resultUpdateData = SaveModifiedQualityData();

            if (resultAddedData && resultUpdateData)
            {
                RefreshQualityData(id, _qualityDataTable);
                Modified = false;
            }

            return resultAddedData & resultUpdateData;
        }

        private bool SaveNewQualityData()
        {
            bool result = true;
            DataTable addedRows = _qualityDataTable.GetChanges(DataRowState.Added);

            if (addedRows == null)
                return result;

            foreach (DataRow row in addedRows.Rows)
            {
                result = _repository.SaveNewQualityData(row);
                if (result)
                {
                    row.AcceptChanges();
                }
                else
                {
                    return result;
                }
            }

            return result;
        }

        private bool SaveModifiedQualityData()
        {
            bool result = true;

            DataTable modifiedRows = _qualityDataTable.GetChanges(DataRowState.Modified);

            if (modifiedRows == null)
                return result;

            foreach (DataRow row in modifiedRows.Rows)
            {
                result = _repository.UpdateQualityData(row);
                if (result)
                {
                    row.AcceptChanges();
                }
                else
                {
                    return result;
                }
            }

            return result;
        }

    }
}
