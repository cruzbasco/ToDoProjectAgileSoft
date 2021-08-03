using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTools
{
    public static class Encrypt
    {
        public static string EncryptPassword(string password)
        {
            using MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] data = md5.ComputeHash(utf8.GetBytes(password));

            return Convert.ToBase64String(data);
        }
    }
}
