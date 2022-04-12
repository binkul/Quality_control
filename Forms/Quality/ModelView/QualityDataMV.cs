using GalaSoft.MvvmLight.Command;
using Quality_Control.Forms.Quality.Command;
using Quality_Control.Service;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityDataMV
    {
        private ICommand _delQualityData;

        private readonly QualityDataService _service = new QualityDataService();
        private readonly DataTable _qualityDataTable;
        public long DataGriddRowIndex { get; set; } = 0;
        public DataView QualityDataView { get; }
        public RelayCommand<InitializingNewItemEventArgs> OnInitializingNewBrookfieldCommand { get; set; }


        public QualityDataMV()
        {
            OnInitializingNewBrookfieldCommand = new RelayCommand<InitializingNewItemEventArgs>(this.OnInitializingNewBrookfieldCommandExecuted);
            _qualityDataTable = _service.GetQualityDataById(-1);
            QualityDataView = new DataView(_qualityDataTable);
        }

        public void RefreshQualityData(long id)
        {
            _service.RefreshQualityData(id, _qualityDataTable);
        }

        public void OnInitializingNewBrookfieldCommandExecuted(InitializingNewItemEventArgs e)
        {
            //var row = _windowEditMV.ActualRow;
            //var id = Convert.ToInt64(row["id"]);
            //var date = Convert.ToDateTime(row["created"]);

            //var maxId = _service.GetTable.AsEnumerable()
            //    .Where(x => x.RowState != DataRowState.Deleted)
            //    .Select(x => x["id"])
            //    .DefaultIfEmpty(-1)
            //    .Max(x => x);

            //var visType = ViscosityType.Brookfield;

            //switch (_profileType)
            //{
            //    case ViscosityType.Brookfield:
            //        visType = ViscosityType.Brookfield;
            //        break;
            //    case ViscosityType.BrookProfile:
            //        visType = ViscosityType.Brookfield;
            //        break;
            //    case ViscosityType.BrookFull:
            //        visType = ViscosityType.Brookfield;
            //        break;
            //    case ViscosityType.BrookfieldX:
            //        visType = ViscosityType.BrookfieldX;
            //        break;
            //    case ViscosityType.Krebs:
            //        visType = ViscosityType.Krebs;
            //        break;
            //    case ViscosityType.ICI:
            //        visType = ViscosityType.ICI;
            //        break;
            //    default:
            //        visType = ViscosityType.Brookfield;
            //        break;
            //}

            //var view = e.NewItem as DataRowView;
            //view.Row["id"] = Convert.ToInt64(maxId) + 1;
            //view.Row["labbook_id"] = id;
            //view.Row["vis_type"] = visType;
            //view.Row["date_created"] = date;
            //view.Row["date_update"] = DateTime.Now;
        }

        public ICommand DeleteQualityDataButton
        {
            get
            {
                if (_delQualityData == null) _delQualityData = new DeleteQualityDataButton(this);
                return _delQualityData;
            }
        }

        public void DeleteQualityData()
        {

        }
    }
}
