using System.Security.Cryptography;
using System.Text;

namespace Quality_Control.Security
{
    public class Encrypt
    {
        public static string MD5Encrypt(string value)
        {
            MD5CryptoServiceProvider crypt = new MD5CryptoServiceProvider();
            byte[] data = Encoding.ASCII.GetBytes(value);
            data = crypt.ComputeHash(data);
            string encrypt = "";
            for (var i = 0; i < data.Length; i++)
            {
                encrypt += data[i].ToString("x2").ToLower();
            }

            return encrypt;
        }
    }
}
