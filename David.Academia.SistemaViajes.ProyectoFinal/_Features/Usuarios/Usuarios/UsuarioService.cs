using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios
{
    public class UsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Usuario> _usuarioRepository;
        private readonly UsuarioDomain _dominio;
        private readonly AuthDomain _authDomain;

        public UsuarioService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, UsuarioDomain usuarioDomain, AuthDomain authDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _mapper = mapper;
            _usuarioRepository = _unitOfWork.Repository<Usuario>();
            _dominio = usuarioDomain;
            _authDomain = authDomain;
        }
        public async Task<Respuesta<UsuarioDto>> CrearUsuario(int usuarioCreaId, UsuarioDto usuarioDto)
        {

            var respuesta = new Respuesta<UsuarioDto>();

            var usuarioValidado = _dominio.ValidarCreacionUsuario(usuarioDto);

            if (!usuarioValidado.Valido)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = usuarioValidado.Mensaje;

                return respuesta;
            }
            if (!await _unitOfWork.Repository<Rol>().AsQueryable().AsNoTracking().AnyAsync(r => r.RolId == usuarioDto.RolId))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Rol");

                return respuesta;
            }
            if (usuarioDto.ColaboradorId > 0 &&  !await _unitOfWork.Repository<Colaborador>().AsQueryable().AsNoTracking().AnyAsync(c => c.ColaboradorId == usuarioDto.ColaboradorId))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "colaborador");

                return respuesta;
            }


            usuarioDto = usuarioValidado.Datos!;

            if (await _usuarioRepository.AsQueryable().AnyAsync(u => u.Nombre.ToLower() == usuarioDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            var usuarioACrear = _mapper.Map<Usuario>(usuarioDto);
            usuarioACrear.UsuarioCrea = usuarioCreaId;

            usuarioACrear.ClaveHash = _authDomain.EncriptarClave(usuarioDto.Clave);

            try
            {

                await _usuarioRepository.AddAsync(usuarioACrear);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<UsuarioDto>(usuarioACrear);
                respuesta.Mensaje = Mensajes.EntidadGuardada;
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

        public async Task<Respuesta<List<UsuarioDto>>> ObtenerUsuarios()
        {
            var respuesta = new Respuesta<List<UsuarioDto>>();
            try
            {
                var usuarios = await _usuarioRepository.AsQueryable().ToListAsync();

                if (usuarios.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
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
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
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

        public async Task<Respuesta<UsuarioDto>> ObtenerUsuarioPorId(int usuarioId)
        {
            var respuesta = new Respuesta<UsuarioDto>();
            try
            {
                var usuario = await _usuarioRepository.AsQueryable().FirstOrDefaultAsync(u => u.UsuarioId == usuarioId && u.Activo == true);

                if (usuario == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

                respuesta.Datos = usuarioDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
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

        public async Task<Respuesta<UsuarioDto>> ActualizarUsuario(int usuarioActualizaId, UsuarioDto usuario)
        {
            var respuesta = new Respuesta<UsuarioDto>();
            try
            {
                var usuarioEncontrado = await _usuarioRepository.AsQueryable().FirstOrDefaultAsync(u => u.UsuarioId == usuario.UsuarioId);

                if (usuarioEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Usuario no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(usuario, usuarioEncontrado);
                usuarioEncontrado.UsuarioActualiza = usuarioActualizaId;
                usuarioEncontrado.FechaActualizacion = DateTime.Now;

                await _unitOfWork.SaveChangesAsync();

                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<UsuarioDto>(usuarioEncontrado);
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
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

        public async Task<Respuesta<bool>> EstadoUsuario(int usuarioId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var usuarioEncontrado = await _usuarioRepository.AsQueryable().FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (usuarioEncontrado == null)
                {
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = Mensajes.EntidadActivada;
                }
                else
                {
                    respuesta.Mensaje = Mensajes.EntidadInactivada;
                }

                usuarioEncontrado.Activo = estado;
                respuesta.Datos = true;
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
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
