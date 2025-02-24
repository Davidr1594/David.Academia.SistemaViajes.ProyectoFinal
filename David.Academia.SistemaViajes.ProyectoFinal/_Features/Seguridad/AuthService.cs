using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad
{
    public class AuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Usuario> _usuarioRepository;

        public AuthService(UnitOfWorkBuilder unitOfWorkBuilder)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _usuarioRepository = _unitOfWork.Repository<Usuario>();
        }

        public async Task<Respuesta<bool>> AutenticarUsuario(string clave, string usuario)
        {
            var respuesta = new Respuesta<bool>();

            if(string.IsNullOrWhiteSpace(clave))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El campo Clave es requerido.";
                return respuesta;
            }
            if (string.IsNullOrWhiteSpace(usuario))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El campo Usuario es requerido.";
                return respuesta;
            }
            try
            {
                var usuarioEcontrado = await _usuarioRepository.AsQueryable().FirstOrDefaultAsync(u => u.Nombre == usuario);
                if (usuarioEcontrado == null)
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "No se encontró un usuario con este nombre.";
                    return respuesta;
                }
                var claveEncriptada = SeguridadHelper.EncriptarClave(clave);

                if (!usuarioEcontrado.ClaveHash.SequenceEqual(claveEncriptada))
                {
                    respuesta.Datos = false;
                    respuesta.Mensaje = "La clave no es valida.";
                    return respuesta;
                }
                respuesta.Mensaje = "Accesso Correcto";
                respuesta.Datos = true;
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
