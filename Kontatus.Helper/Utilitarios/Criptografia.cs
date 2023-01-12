using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Kontatus.Helper.Utilitarios
{
    public static class Criptografia
    {
        public static string RandomString(int length, bool justNumber)
        {
            const string chars = "abcdefghijklmnoqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string charsNumber = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(justNumber ? charsNumber : chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Cripto(string x)
        {
            string hash;

            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, x);
            }

            return hash;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
