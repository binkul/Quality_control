using System.Collections.Generic;

namespace Quality_Control.Security
{
    public class User
    {
        public long Id { get; }
        public string Name { get; }
        public string Surname { get; }
        public string Email { get; }
        public string Login { get; }
        public string Permission { get; }
        public string Identifier { get; }
        public bool IsActive { get; }

        public User(long id, string name, string surname, string email, string login, string permission, string identifier, bool isActive)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            Login = login;
            Permission = permission;
            Identifier = identifier;
            IsActive = isActive;
        }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id == user.Id &&
                   Name == user.Name &&
                   Surname == user.Surname &&
                   Email == user.Email &&
                   Login == user.Login &&
                   Permission == user.Permission &&
                   Identifier == user.Identifier &&
                   IsActive == user.IsActive;
        }

        public override int GetHashCode()
        {
            int hashCode = -917213164;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Login);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Permission);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Identifier);
            hashCode = hashCode * -1521134295 + IsActive.GetHashCode();
            return hashCode;
        }
    }
}
