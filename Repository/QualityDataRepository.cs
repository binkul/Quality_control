using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace Quality_Control.Repository
{
    public class QualityDataRepository
    {
        private readonly string _qualityDataQuery = "Select DATEDIFF(day, production_date, measure_date) as days_distance, d.* from LabBook.dbo.QualityControlData d " +
            "left join LabBook.dbo.QualityControl q on d.quality_id=q.id Where d.quality_id=XXXX Order by days_distance, d.quality_id";
        private readonly string _getFieldsToShow = "Select active_fields From LabBook.dbo.QualityControlFields Where labbook_id=";
        private readonly string _deleteQualityDataByIdQuery = "Delete From LabBook.dbo.QualityControlData Where id=";
        private readonly string _deleteQualityDataByQualityIdQuery = "Delete From LabBook.dbo.QualityControlData Where quality_id=";

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
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
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

            return result;
        }

        public bool DeleteQualityDataById(long id)
        {
            bool result = true;
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.CommandText = _deleteQualityDataByIdQuery + id.ToString();
                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return result;

        }

        public bool DeleteQualityDataByQualityId(long qualityId)
        {
            bool result = true;
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.CommandText = _deleteQualityDataByQualityIdQuery + qualityId.ToString();
                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return result;

        }

        public bool SaveNewQualityData(DataRow row)
        {
            bool result = true;
            StringBuilder kwerStart = new StringBuilder("Insert Into LabBook.dbo.QualityControlData(");
            StringBuilder kwerEnd = new StringBuilder(" Values(");
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                int index = 1;
                foreach (DataColumn column in row.Table.Columns)
                {
                    string field = column.ColumnName;
                    if (field != "days_distance" && field != "id")
                    {
                        kwerStart.Append(field);
                        kwerStart.Append(", ");
                        kwerEnd.Append("@a");
                        kwerEnd.Append(index.ToString());
                        kwerEnd.Append(", ");

                        cmd.Parameters.AddWithValue("@a" + index.ToString(), row[field]);
                        index++;
                    }
                }
                kwerStart.Append("quality_id)");
                kwerEnd.Append("@quality_id)");

                cmd.CommandText = kwerStart.ToString() + kwerEnd.ToString();
                cmd.Parameters.AddWithValue("@quality_id", row["quality_id"]);
                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return result;
        }

        public bool UpdateQualityData(DataRow row)
        {
            bool result = true;
            StringBuilder kwerStart = new StringBuilder("Update LabBook.dbo.QualityControlData Set ");
            StringBuilder kwerEnd = new StringBuilder(" Where id=@id");
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                int index = 1;
                foreach (DataColumn column in row.Table.Columns)
                {
                    string field = column.ColumnName;
                    if (field != "days_distance" && field != "quality_id")
                    {
                        kwerStart.Append(field);
                        kwerStart.Append("=");
                        kwerStart.Append("@a");
                        kwerStart.Append(index.ToString());
                        kwerStart.Append(", ");

                        cmd.Parameters.AddWithValue("@a" + index.ToString(), row[field]);
                        index++;
                    }
                }

                kwerStart.Remove(kwerStart.Length - 2, 2);
                cmd.CommandText = kwerStart.ToString() + kwerEnd.ToString(); ;
                cmd.Parameters.AddWithValue("@id", row["id"]);
                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
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
