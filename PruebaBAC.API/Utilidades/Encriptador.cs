using System.Security.Cryptography;
using System.Text;

namespace PruebaBAC.API.Utilidades
{
    public static class Encriptador
    {
        public static string ConvertirSHA256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }
    }
}