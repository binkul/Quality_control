using Quality_Control.Dto;
using Quality_Control.Security;
using Quality_Control.Commons;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace Quality_Control.Repository
{
    public enum RegisterStatus
    {
        Ok,
        Exist,
        Error
    }

    public class UserRepository : RepositoryCommon<UserDto>
    {
        private const string _loginQuery = "Select us.id, us.name, us.surname, us.e_mail, us.login, us.permission, us.identifier, us.active "
                                   + "From LabBook.dbo.Users us Where us.login='@username' and us.password='@password'";
        private const string _existQuery = "Select Count(1) From LabBook.dbo.Users usr Where usr.login=@username";
        public static readonly string _registerQuery = "Insert Into LabBook.dbo.Users(name, surname, e_mail, login, password, "
                                   + "permission, identifier, active, date) Values(@name, @surname, @e_mail, @login, @password, "
                                   + "@permission, @identifier, @active, @date)";


        public User GetUserByLoginAndPassword(string login, string password)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    string query = _loginQuery.Replace("@username", login);
                    query = query.Replace("@password", Encrypt.MD5Encrypt(password));

                    SqlCommand sqlCmd = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        long id = Convert.ToInt64(reader[0]);
                        string name = Convert.ToString(reader[1]);
                        string surname = Convert.ToString(reader[2]);
                        string email = Convert.ToString(reader[3]);
                        string log = Convert.ToString(reader[4]);
                        string permission = Convert.ToString(reader[5]);
                        string identifier = Convert.ToString(reader[6]);
                        bool isActive = Convert.ToBoolean(reader[7]);
                        user = new User(id, name, surname, email, login, permission, identifier, isActive);
                    }

                    reader.Close();
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
                    connection.Close();
                }
            }

            return user;
        }

        public RegisterStatus ExistUserByLogin(string login)
        {
            var result = RegisterStatus.Ok;

            using (var connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    var sqlCmd = new SqlCommand(_existQuery, connection) { CommandType = CommandType.Text };
                    sqlCmd.Parameters.AddWithValue("@username", login);
                    connection.Open();

                    if (Convert.ToInt32(sqlCmd.ExecuteScalar()) >= 1)
                    {
                        _ = MessageBox.Show("Użytkownik o loginie: '" + login + "' istnieje już w bazie. Użyj innego loginu do rejestracji.",
                            "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        result = RegisterStatus.Exist;
                    }
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                    result = RegisterStatus.Error;
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                    result = RegisterStatus.Error;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }

        public override UserDto Save(UserDto data)
        {
            RegisterStatus result = RegisterStatus.Ok;

            using (SqlConnection connection = new SqlConnection(Application.Current.FindResource("ConnectionString").ToString()))
            {
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(_registerQuery, connection) { CommandType = CommandType.Text };
                    sqlCmd.Parameters.AddWithValue("@name", data.Name);
                    sqlCmd.Parameters.AddWithValue("@surname", data.Surname);
                    sqlCmd.Parameters.AddWithValue("@e_mail", data.Email);
                    sqlCmd.Parameters.AddWithValue("@login", data.Login);
                    sqlCmd.Parameters.AddWithValue("@password", Encrypt.MD5Encrypt(data.Password));
                    sqlCmd.Parameters.AddWithValue("@permission", data.Permission);
                    sqlCmd.Parameters.AddWithValue("@identifier", data.Identifier);
                    sqlCmd.Parameters.AddWithValue("@active", data.Active);
                    sqlCmd.Parameters.AddWithValue("@date", data.Date);
                    connection.Open();
                    sqlCmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony, błąd w nazwie serwera lub dostępie do bazy: '" + ex.Message + "'",
                        "Błąd połaczenia", MessageBoxButton.OK, MessageBoxImage.Error);
                    result = RegisterStatus.Error;
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Problem z połączeniem z serwerem. Prawdopodobnie serwer jest wyłączony: '" + ex.Message + "'",
                        "Błąd połączenia", MessageBoxButton.OK, MessageBoxImage.Error);
                    result = RegisterStatus.Error;
                }
                finally
                {
                    connection.Close();
                }
            }

            if (result == RegisterStatus.Ok)
            {
                User user = GetUserByLoginAndPassword(data.Login, data.Password);
                data.Id = user.Id;
            }

            return data;
        }
    }
}
