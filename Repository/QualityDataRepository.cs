using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Quality_Control.Repository
{
    public class QualityDataRepository
    {
        private readonly string _qualityDataQuery = "Select DATEDIFF(day, production_date, measure_date) as days_distance, d.* from LabBook.dbo.QualityControlData d " +
            "left join LabBook.dbo.QualityControl q on d.quality_id=q.id Where d.quality_id=XXXX Order by days_distance, d.quality_id";
        private readonly string _getFieldsToShow = "Select active_fields From LabBook.dbo.QualityControlFields Where labbook_id=";

        public DataTable GetQualityDataByQualityId(long id)
        {
            DataTable table = new DataTable();

            using (var connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    string kwer = _qualityDataQuery.Replace("XXXX", id.ToString());
                    SqlDataAdapter adapter = new SqlDataAdapter(kwer, connection);
                    adapter.Fill(table);
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return table;
        }

        public void LoadQualityDataById(DataTable table, long id)
        {
            using (var connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    string kwer = _qualityDataQuery.Replace("XXXX", id.ToString());
                    SqlDataAdapter adapter = new SqlDataAdapter(kwer, connection);
                    adapter.Fill(table);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public String GetActiveFieldsByLabBookId(long labBookId)
        {
            string result = "";
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.CommandText = _getFieldsToShow + labBookId.ToString();
                cmd.Connection = connection;

                connection.Open();
                result = (string)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return result;
        }

    }
}
