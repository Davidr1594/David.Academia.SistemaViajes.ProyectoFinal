using AutoMapper;
using Azure;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios
{
    public class UsuarioService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public  UsuarioService(SistemaTransporteDrContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }
        public async Task<Respuesta<UsuarioDto>>CrearUsuario(int usuarioCreaId, UsuarioDto usuarioDto)
        {

            var respuesta = new Respuesta<UsuarioDto>();

            if (usuarioDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un usuario valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(usuarioDto.Nombre) || string.IsNullOrWhiteSpace(usuarioDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del usuario es requerido.";
                return respuesta;
            }
            if (await _context.Roles.AnyAsync(u => u.Nombre.ToLower() == usuarioDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un usuario con este nombre.";
                return respuesta;
            }

            var usuarioACrear = _mapper.Map<Usuario>(usuarioDto);
            usuarioACrear.UsuarioCrea = usuarioCreaId;

            if (usuarioDto.ColaboradorId <= 0)
            {
                usuarioACrear.ColaboradorId = null;
            }

            usuarioACrear.ClaveHash = SeguridadHelper.EncriptarClave(usuarioDto.Clave);

            try
            {


                await _context.Usuarios.AddAsync(usuarioACrear);
                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<UsuarioDto>(usuarioACrear);
                respuesta.Mensaje = ("Usuario creado con éxito.");
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

        public async Task<Respuesta<List<UsuarioDto>>> ObtenerUsuarios()
        {
            var respuesta = new Respuesta<List<UsuarioDto>>();
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();

                if (usuarios.Count == 0)
                { 
                    respuesta.Valido =false;
                    respuesta.Mensaje = "No hay usuarios";
                    return respuesta;
                }

                var usuariosDto = new List<UsuarioDto>();

                foreach (var usuario in usuarios)
                { 
                    usuariosDto.Add(_mapper.Map<UsuarioDto>(usuario));
                }

                respuesta.Datos = usuariosDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al guardar en la base de datos.";
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

        public async Task<Respuesta<UsuarioDto>> ObtenerUsuarioPorId(int usuarioId)
        {
            var respuesta = new Respuesta<UsuarioDto>();
            try
            {
                var usuario  = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (usuario == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Usuario no encontrado.";
                }
                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                respuesta.Datos = usuarioDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al guardar en la base de datos.";
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

        public async Task<Respuesta<UsuarioDto>> ActualizarUsuario(int usuarioActualizaId, UsuarioDto usuario)
        {
            var respuesta = new Respuesta<UsuarioDto>();
            try
            {
                var usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuario.UsuarioId);

                if (usuarioEncontrado == null) 
                {
                    respuesta.Mensaje = "Usuario no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(usuario, usuarioEncontrado);
                usuarioEncontrado.UsuarioActualiza = usuarioActualizaId;
                usuarioEncontrado.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<UsuarioDto>(usuarioEncontrado); 
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al guardar en la base de datos.";
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

        public async Task<Respuesta<bool>> EstadoUsuario(int usuarioId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var usuarioEncontrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (usuarioEncontrado == null)
                {
                    respuesta.Mensaje = "Usuario no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Usuario ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Usuario ha sido inactivado.";
                }

                usuarioEncontrado.Activo = estado;
                await _context.SaveChangesAsync();


            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al actualizar en la base de datos.";
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
