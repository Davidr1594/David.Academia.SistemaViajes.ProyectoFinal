using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad
{
    public class AuthService
    {
        private readonly SistemaTransporteDrContext _context;

        public AuthService(SistemaTransporteDrContext context)
        {
            _context = context;
        }

        public async Task<Respuesta<bool>> AutenticarUsuario(string clave, string usuario)
        {
            var respuesta = new Respuesta<bool>();

            if(string.IsNullOrEmpty(clave) || string.IsNullOrWhiteSpace(clave))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El campo Clave es requerido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrWhiteSpace(usuario))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El campo Usuario es requerido.";
                return respuesta;
            }
            try
            {
                var usuarioEcontrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == usuario);
                if (usuarioEcontrado == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No se encontró un usuario con este nombre.";
                    return respuesta;
                }
                var claveEncriptada = SeguridadHelper.EncriptarClave(clave);

                if (!usuarioEcontrado.ClaveHash.SequenceEqual(claveEncriptada))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "La clave no es valida.";
                    return respuesta;
                }
                respuesta.Mensaje = "Accesso Correcto";
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
