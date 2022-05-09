using Quality_Control.Commons;
using Quality_Control.Forms.AddNew.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quality_Control.Repository
{
    public class ProductRepository
    {
        private readonly string _getAllActiveProductQuery = "Select p.id, p.labbook_id, p.name, p.hp_index, p.description, p.is_danger, " +
            "p.is_archive, p.is_experimetPhase, p.product_price_id, p.product_type_id, p.product_gloss_id, p.created, p.login_id, " +
            "ISNULL(f.active_fields, '') as active_fields From LabBook.dbo.Product p Left Join QualityControlFields f On p.labbook_id=f.labbook_id " +
            "Where is_archive= 'false' AND is_experimetPhase = 'false' Order By name";
        private readonly string _existActiveFieldsQuary = "Select Count(*) as num From LabBook.dbo.QualityControlFields Where labbook_id=";
        private readonly string _getActiveFieldsQuery = "Select active_fields From LabBook.dbo.QualityControlFields Where labbook_id=";

        public List<ProductModel> GetOnlyActiveProduct()
        {
            List<ProductModel> quality = new List<ProductModel>();

            using (SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(_getAllActiveProductQuery, connection);
                    connection.Open();
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            long id = reader.GetInt64(0);
                            long labBookId = reader.GetInt64(1);
                            string name = reader.GetString(2);
                            string index = !reader.GetValue(3).Equals(DBNull.Value) ? reader.GetString(3) : "";
                            string description = !reader.GetValue(4).Equals(DBNull.Value) ? reader.GetString(3) : "";
                            bool isDanger = reader.GetBoolean(5);
                            bool isArchive = reader.GetBoolean(6);
                            bool isExperiment = reader.GetBoolean(7);
                            int priceId = reader.GetInt32(8);
                            int typeId = reader.GetInt32(9);
                            int glossId = reader.GetInt32(10);
                            DateTime date = reader.GetDateTime(11);
                            int loginId = reader.GetInt32(12);
                            string fields = reader.GetValue(13).Equals(DBNull.Value) ? reader.GetString(13) : "";

                            ProductModel product = new ProductModel(id, labBookId, name, index, description, isDanger, 
                                isArchive, isExperiment, priceId, typeId, glossId, date, loginId, fields);
                            quality.Add(product);
                        }
                        reader.Close();
                    }
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Problem z odczytem z tabeli 'Product' - błąd w zapytaniu, brak danych lub problem z serwerem. + '" + ex.Message + "'",
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

        public bool ExistFieldsByLabBookId(long labBookId)
        {
            int result = 0;

            using (SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_existActiveFieldsQuary + labBookId, connection);
                    connection.Open();
                    result = (int)command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Błąd odczytu danych z tabeli QualityControlFields: '" + ex.Message + "'. Błąd odczytu.",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'. Błąd odczytu.",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return result > 0;
        }

        public string GetActiveFieldsForProduct(long labBookId)
        {
            object fields = null;

            using (SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand(_getActiveFieldsQuery + labBookId, connection);
                    connection.Open();
                    fields = command.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Błąd odczytu danych z tabeli QualityControlFields: '" + ex.Message + "'. Błąd odczytu.",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'. Błąd odczytu.",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }

            return fields != null ? fields.ToString() : "";
        }

    }
}
