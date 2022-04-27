using Quality_Control.Commons;
using Quality_Control.Forms.Quality.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Quality_Control.Repository
{
    public class QualityRepository
    {
        private readonly string _getAllByYearQuery = "Select q.id, q.number, q.product_name, q.product_index, q.labbook_id, q.remarks, q.active_fields, q.production_date, q.login_id, u.login, q.product_type_id, t.name " +
            "From LabBook.dbo.QualityControl q left join LabBook.dbo.Users u on q.login_id= u.id left join LabBook.dbo.CmbPaintType t on q.product_type_id= t.id " +
            "Where YEAR(q.production_date)=XXXX Order By q.number";
        private readonly string _getByNumberAndYear = "Select q.id, q.number, q.product_name, q.product_index, q.labbook_id, q.remarks, q.active_fields, "
            + " q.production_date, q.login_id, u.login, q.product_type_id, t.name From LabBook.dbo.QualityControl q left join LabBook.dbo.Users u on "
            + " q.login_id=u.id left join LabBook.dbo.CmbPaintType t on q.product_type_id=t.id Where q.number=xxxx AND YEAR(q.production_date)=yyyy";
        private readonly string _getFieldsByLabbokId = "Select active_fields From LabBook.dbo.QualityControlFields Where labbook_id=";
        private readonly string _allYearsQuery = "Select Distinct YEAR(production_date) as year from Labbook.dbo.QualityControl Order by YEAR(production_date)";
        private readonly string _deleteQualityByIdQuery = "Delete From LabBook.dbo.QualityControl Where id=";
        private readonly string _qualityUpdateQuery = "Update LabBook.dbo.QualityControl Set production_date=@production_date, number=@number, product_name=@product_name, product_index=@product_index, " +
            "labbook_id=@labbook_id, product_type_id=@production_type_id, remarks=@remarks, active_fields=@active_fields, login_id=@login_id Where id = @id";
        private readonly string _existByNumberAndYear = "Select Count(*) as exist From LabBook.dbo.QualityControl Where number=xxxx AND YEAR(production_date)=yyyy";
        private readonly string _saveNewQuality = "Insert Into LabBook.dbo.QualityControl(production_date, number, product_name, product_index, labbook_id, product_type_id, remarks, active_fields, login_id) " +
            " Values(@production_date, @number, @product_name, @product_index, @labbook_id, @production_type_id, @remarks, @active_fields, @login_id)";


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
                            int number = reader.GetInt32(1);
                            string name = reader.GetString(2);
                            string index = !reader.GetValue(3).Equals(DBNull.Value) ? reader.GetString(3) : "";
                            long labBookId = reader.GetInt64(4);
                            string remarks = !reader.GetValue(5).Equals(DBNull.Value) ? reader.GetString(5) : "";
                            string fields = !reader.GetValue(6).Equals(DBNull.Value) ? reader.GetString(6) : "";
                            DateTime date = reader.GetDateTime(7);
                            int loginId = reader.GetInt32(8);
                            string login = !reader.GetValue(9).Equals(DBNull.Value) ? reader.GetString(9) : "";
                            int productTypeId = reader.GetInt32(10);
                            string productTypeName = !reader.GetValue(11).Equals(DBNull.Value) ? reader.GetString(11) : "";

                            QualityModel qualityModel = new QualityModel(id, number, name, index, labBookId, productTypeId, productTypeName,
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

        public QualityModel GetByNumberAndYear(int number, int year)
        {
            QualityModel quality = new QualityModel();
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                string query = _getByNumberAndYear.Replace("xxxx", number.ToString());
                cmd.CommandText = query.Replace("yyyy", year.ToString());
                cmd.Connection = connection;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        quality.Number = reader.GetInt32(1);
                        quality.ProductName = reader.GetString(2);
                        quality.ProductIndex = !reader.GetValue(3).Equals(DBNull.Value) ? reader.GetString(3) : "";
                        quality.LabBookId = reader.GetInt64(4);
                        quality.Remarks = !reader.GetValue(5).Equals(DBNull.Value) ? reader.GetString(5) : "";
                        quality.ActiveDataFields = !reader.GetValue(6).Equals(DBNull.Value) ? reader.GetString(6) : "";
                        quality.ProductionDate = reader.GetDateTime(7);
                        quality.LoginId = reader.GetInt32(8);
                        quality.Login = !reader.GetValue(9).Equals(DBNull.Value) ? reader.GetString(9) : "";
                        quality.ProductTypeId = reader.GetInt32(10);
                        quality.ProductTypeName = !reader.GetValue(11).Equals(DBNull.Value) ? reader.GetString(11) : "";
                        quality.Id = reader.GetInt64(0);
                    }
                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z zapytaniem do tabeli QualityControl (GetByNumberAndYear) - niewłaściwa kwerenda lub brak połaczenia z serwerem: '" + ex.Message + "'",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z odczytu danych z tabeli QualityControl.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return quality;
        }

        public DbResponse ExistByNumberAndYear(int number, int year)
        {
            DbResponse result = DbResponse.FALSE;
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                string query = _existByNumberAndYear.Replace("xxxx", number.ToString());
                cmd.CommandText = query.Replace("yyyy", year.ToString());
                cmd.Connection = connection;

                connection.Open();
                if ((int)cmd.ExecuteScalar() > 0)
                    result = DbResponse.TRUE;
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z zapytaniem do tabeli QualityControl (ExistByNumberAndYear) - niewłaściwa kwerenda lub brak połaczenia z serwerem: '" + ex.Message + "'",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = DbResponse.ERROR;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu odczytu danych z tabeli QualityControl.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = DbResponse.ERROR;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return result;
        }

        public string GetActiveFieldsByLabBookId(long labBookId)
        {
            string result = "";
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.CommandText = _getFieldsByLabbokId + labBookId.ToString();
                cmd.Connection = connection;

                connection.Open();
                result = (string)cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z zapytaniem do tabeli QualityControlFields (GetActiveFieldsByLabBookId) - niewłaściwa kwerenda lub brak połaczenia z serwerem: '" + ex.Message + "'.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd odczytu z QualityControlFields.",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public List<int> GetAllYears()
        {
            List<int> years = new List<int>();

            using (var connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_allYearsQuery, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int year = reader.GetInt32(0);
                            years.Add(year);
                        }
                        reader.Close();
                    }
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

            return years;

        }

        public bool DeleteQualityById(long id)
        {
            bool result = true;
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.CommandText = _deleteQualityByIdQuery + id.ToString();
                cmd.Connection = connection;

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
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

        public QualityModel Save(QualityModel quality)
        {
            QualityModel result = new QualityModel();
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.Connection = connection;
                cmd.CommandText = _saveNewQuality;
                cmd.Parameters.AddWithValue("@production_date", quality.ProductionDate);
                cmd.Parameters.AddWithValue("@number", quality.Number);
                cmd.Parameters.AddWithValue("@product_name", quality.ProductName);
                if (!string.IsNullOrEmpty(quality.ProductIndex))
                    cmd.Parameters.AddWithValue("@product_index", quality.ProductIndex);
                else
                    cmd.Parameters.AddWithValue("@product_index", DBNull.Value);
                cmd.Parameters.AddWithValue("@labbook_id", quality.LabBookId);
                cmd.Parameters.AddWithValue("@production_type_id", quality.ProductTypeId);
                if (!string.IsNullOrEmpty(quality.Remarks))
                    cmd.Parameters.AddWithValue("@remarks", quality.Remarks);
                else
                    cmd.Parameters.AddWithValue("@remarks", DBNull.Value);
                if (!string.IsNullOrEmpty(quality.ActiveDataFields))
                    cmd.Parameters.AddWithValue("@active_fields", quality.ActiveDataFields);
                else
                    cmd.Parameters.AddWithValue("@active_fields", DefaultData.DefaultDataFields);
                cmd.Parameters.AddWithValue("@login_id", quality.LoginId);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                result = GetByNumberAndYear(quality.Number, quality.ProductionDate.Year);
            }
            catch (SqlException ex)
            {
                _ = MessageBox.Show("Problem z zapisem do tabeli QualityControl (Save) - błąd w kwerendzie lub brak połaczenia z serwerem: '" + ex.Message + "'.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd Save do QualityControl",
                    "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return result;
        }

        public bool Update(QualityModel quality)
        {
            bool result = true;
            SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString());
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd.Connection = connection;
                cmd.CommandText = _qualityUpdateQuery;
                cmd.Parameters.AddWithValue("@production_date", quality.ProductionDate);
                cmd.Parameters.AddWithValue("@number", quality.Number);
                cmd.Parameters.AddWithValue("@product_name", quality.ProductName);
                if (!string.IsNullOrEmpty(quality.ProductIndex))
                    cmd.Parameters.AddWithValue("@product_index", quality.ProductIndex);
                else
                    cmd.Parameters.AddWithValue("@product_index", DBNull.Value);
                cmd.Parameters.AddWithValue("@labbook_id", quality.LabBookId);
                cmd.Parameters.AddWithValue("@production_type_id", quality.ProductTypeId);
                if (!string.IsNullOrEmpty(quality.Remarks))
                    cmd.Parameters.AddWithValue("@remarks", quality.Remarks);
                else
                    cmd.Parameters.AddWithValue("@remarks", DBNull.Value);
                if (!string.IsNullOrEmpty(quality.ActiveDataFields))
                    cmd.Parameters.AddWithValue("@active_fields", quality.ActiveDataFields);
                else
                    cmd.Parameters.AddWithValue("@active_fields", DBNull.Value);
                cmd.Parameters.AddWithValue("@login_id", quality.LabBookId);
                cmd.Parameters.AddWithValue("@id", quality.Id);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
                    "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd z poziomu CreateTable VisRepository.",
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
