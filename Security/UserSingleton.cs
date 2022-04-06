using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quality_Control.Security
{
    public sealed class UserSingleton
    {
        private static volatile UserSingleton _instance = null;
        private static object _syncRoot = new object();
        public static long Id { get; private set; } = 0;
        public static string Name { get; private set; }
        public static string SurName { get; private set; }
        public static string Email { get; private set; }
        public static string Login { get; private set; }
        public static string Permission { get; private set; }
        public static string Identifier { get; private set; }
        public static bool IsActive { get; private set; }

        private UserSingleton(long id, string name, string surname, string email, string login, string permission, string identifier, bool isActive)
        {
            Id = id;
            Name = name;
            SurName = surname;
            Email = email;
            Login = login;
            Permission = permission;
            Identifier = identifier;
            IsActive = isActive;
        }

        public static UserSingleton CreateInstance(long id, string name, string surname, string email, string login, string permission, string identifier, bool isActive)
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new UserSingleton(id, name, surname, email, login, permission, identifier, isActive);
                }
            }

            return _instance;
        }

        public static UserSingleton GetInstance => _instance;
    }
}
