using Quality_Control.Forms.Quality.Model;
using Quality_Control.Repository;
using System.Collections.Generic;
using System.Data;

namespace Quality_Control.Service
{
    public class QualityDataService
    {
        private static readonly List<string> _defaultFields = new List<string>() { "measure_date", "temp", "density", "pH", "vis_1", "vis_5", "vis_20", "disc", "comments" };
        private readonly QualityDataRepository _repository;

        public QualityDataService()
        {
            _repository = new QualityDataRepository();
        }

        public DataTable GetQualityDataById(long id)
        {
            return _repository.GetQualityDataByQualityId(id);
        }

        public void RefreshQualityData(long id, DataTable table)
        {
            table.Rows.Clear();
            _repository.LoadQualityDataById(table, id);
        }

        public List<string> GetActiveFields(QualityModel quality)
        {
            List<string> result = new List<string>();
            string fields = quality.ActiveDataFields;

            if (!string.IsNullOrEmpty(fields))
            {
                string[] tmp =  fields.Split('|');
                result.AddRange(tmp);
            }
            else
                result = _defaultFields;

            return result;
        }

        public void Delete(long id)
        {
            _repository.DeleteQualityDataById(id);
        }
    }
}
