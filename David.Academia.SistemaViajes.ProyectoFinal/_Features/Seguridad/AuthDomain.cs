using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad.Dto;
using System.Security.Cryptography;
using System.Text;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad
{
    public class AuthDomain
    {

        public Respuesta<bool> ValidarCredencialesValidas(UsuarioAuthDto usuario)
        {
            var respuesta = new Respuesta<bool>();

            if (string.IsNullOrWhiteSpace(usuario.clave))
            {
                respuesta.Mensaje = string.Format(Mensajes.DatoNoValidoEspecifico, "clave");
                respuesta.Datos = false;
                return respuesta;
            }
            if (string.IsNullOrWhiteSpace(usuario.nombre))
            {
                respuesta.Mensaje = string.Format(Mensajes.DatoNoValidoEspecifico, "usuario");
                respuesta.Datos = false;
                return respuesta;
            }
            respuesta.Datos = true;

            return respuesta;
        }

        public  byte[] EncriptarClave(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(clave));
            }
        }
    }
}
