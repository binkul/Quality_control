using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Quality_Control.Repository
{
    public class QualityRepository
    {
        private readonly string _getAllByYearQuery = "Select q.id, (CAST(q.number as varchar) + '/' + RIGHT(CAST(DATEPART(yy, q.production_date) as varchar), 2)) " +
            "as number_year , q.number, q.product_name, q.product_index, q.labbook_id, q.remarks, q.active_fields, q.production_date, q.login_id, u.login, q.product_type_id, t.name " +
            "From LabBook.dbo.QualityControl q left join LabBook.dbo.Users u on q.login_id= u.id left join LabBook.dbo.CmbPaintType t on q.product_type_id= t.id " +
            "Where YEAR(q.production_date)=XXXX Order By q.number";

        public SortableObservableCollection<QualityModel> GetAllByYear(int year)
        {
            SortableObservableCollection<QualityModel> quality = new SortableObservableCollection<QualityModel>();

            using (SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    string query = _getAllByYearQuery.Replace("XXXX", year.ToString());
                    SqlCommand sqlCmd = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            long id = reader.GetInt64(0);
                            string yearNumber = reader.GetString(1);
                            int number = reader.GetInt32(2);
                            string name = reader.GetString(3);
                            string index = !reader.GetValue(4).Equals(DBNull.Value) ? reader.GetString(4) : "";
                            long labBookId = reader.GetInt64(5);
                            string remarks = !reader.GetValue(6).Equals(DBNull.Value) ? reader.GetString(6) : "";
                            string fields = !reader.GetValue(7).Equals(DBNull.Value) ? reader.GetString(7) : "";
                            DateTime date = reader.GetDateTime(8);
                            int loginId = reader.GetInt32(9);
                            string login = !reader.GetValue(10).Equals(DBNull.Value) ? reader.GetString(10) : "";
                            int productTypeId = reader.GetInt32(11);
                            string productTypeName = !reader.GetValue(12).Equals(DBNull.Value) ? reader.GetString(12) : "";

                            QualityModel qualityModel = new QualityModel(id, number, yearNumber, name, index, labBookId, productTypeId, productTypeName,
                                remarks, fields, date, loginId, login);
                            quality.Add(qualityModel);
                        }
                        reader.Close();
                    }
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return quality;
        }

    }
}
