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
        private readonly string _getAllActiveProductQuery = "Select id, labbook_id, name, hp_index, description, is_danger, is_archive, is_experimetPhase, product_price_id, product_type_id, " +
            "product_gloss_id, created, login_id From LabBook.dbo.Product Where is_archive='false' AND is_experimetPhase='false' Order By name";

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

                            ProductModel product = new ProductModel(id, labBookId, name, index, description, isDanger, 
                                isArchive, isExperiment, priceId, typeId, glossId, date, loginId);
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

    }
}
