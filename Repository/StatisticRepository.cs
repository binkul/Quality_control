using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace Quality_Control.Repository
{
    public class StatisticRepository
    {
        private readonly string _getStatisticToday = "Select q.product_name as name, 'P' + CAST(q.number as nvarchar) as number, q.labbook_id, q.active_fields, " +
                "d.* from LabBook.dbo.QualityControlData d left join LabBook.dbo.QualityControl q on d.quality_id=q.id Where q.production_date=@date AND " +
                "q.production_date=d.measure_date Order By q.number, d.id";

        public DataTable GetStatisticToday()
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand(_getStatisticToday, connection)
                    };
                    DateTime date = DateTime.Today;
                    adapter.SelectCommand.Parameters.Add("@date", SqlDbType.Date).Value = DateTime.Today;
                    _ = adapter.Fill(table);
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Błąd odczytu danych z tabeli QualityControlData: '" + ex.Message + "'. Błąd z poziomu GetStatisticToday.",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd odczytu z poziomu GetStatisticToday.",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return table;
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
                    if (field != "name" && field != "number" && field != "id" && field != "active_fields" && field != "labbook_id")
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
                cmd.CommandText = kwerStart.ToString() + kwerEnd.ToString();
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
