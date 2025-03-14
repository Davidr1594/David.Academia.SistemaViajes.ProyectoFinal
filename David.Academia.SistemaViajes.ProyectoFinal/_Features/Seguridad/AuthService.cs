using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad
{
    public class AuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Usuario> _usuarioRepository;
        private readonly AuthDomain _authDomain;
       

        public AuthService(UnitOfWorkBuilder unitOfWorkBuilder, AuthDomain authDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _usuarioRepository = _unitOfWork.Repository<Usuario>();
            _authDomain = authDomain;

        }

        public async Task<Respuesta<UsuarioDto>> AutenticarUsuario(UsuarioAuthDto usuario)
        {
            var respuesta = new Respuesta<UsuarioDto>();

            var validarCredenciales = _authDomain.ValidarCredencialesValidas(usuario);

            if (!validarCredenciales.Datos)
            {
                respuesta.Valido = validarCredenciales.Valido;
                respuesta.Mensaje = validarCredenciales.Mensaje;
  
                return respuesta;
            }

            try
            {
                var usuarioEcontrado = await _usuarioRepository.AsQueryable().FirstOrDefaultAsync(u => u.Nombre == usuario.nombre);
                if (usuarioEcontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.NoSeEncontroEntidadNombre, "un usuario");
                    respuesta.Valido = false;
                    return respuesta;
                }
                var claveEncriptada = _authDomain.EncriptarClave(usuario.clave);

                if (!usuarioEcontrado.ClaveHash.SequenceEqual(claveEncriptada))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.ContraseniaIncorrecta;
                    return respuesta;
                }
                var usuarioDto = new UsuarioDto() { 
                    Nombre = usuarioEcontrado.Nombre,
                    Email = usuarioEcontrado.Email,
                    RolId = usuarioEcontrado.RolId,
                    Activo = usuarioEcontrado.Activo,
                    UsuarioId = usuarioEcontrado.UsuarioId,
                    ColaboradorId = usuarioEcontrado.ColaboradorId
                };

                respuesta.Mensaje = Mensajes.AccessoCorrecto;
                respuesta.Valido = true;
                respuesta.Datos = usuarioDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
