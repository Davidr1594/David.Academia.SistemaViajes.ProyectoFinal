using System.Security.Cryptography;
using System.Text;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad
{
    public static class SeguridadHelper
    {
        public static byte[] EncriptarClave(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(clave));
            }
        }
    }
}
