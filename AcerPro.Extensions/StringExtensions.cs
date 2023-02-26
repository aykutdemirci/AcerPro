using System.Security.Cryptography;
using System.Text;

namespace AcerPro.Extensions
{
    public static class StringExtensions
    {
        public static string ToMd5(this string value) 
        {
            byte[] btr = GetMD5Hash(value);
            StringBuilder sb = new StringBuilder();
            foreach (byte bt in btr)
            {
                sb.Append(bt.ToString("x2").ToLower());
            }
            return sb.ToString();
        }

        private static byte[] GetMD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}
