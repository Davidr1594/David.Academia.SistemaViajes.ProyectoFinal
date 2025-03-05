using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Roles
{
    public class RolService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Rol> _rolRepository;
        private readonly RolDomain _rolDomain;


        public RolService(IMapper mapper, UnitOfWorkBuilder unitOfWorkBuilder, RolDomain rolDomain)
        {
            _mapper = mapper;

            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _rolRepository = _unitOfWork.Repository<Rol>();
            _rolDomain = rolDomain;
        }

        public async Task<Respuesta<RolDto>> CrearRol(RolDto rolDto)
        {
            var respuesta = new Respuesta<RolDto>();

            var respuestaValidarEntrada = _rolDomain.ValidarDatosDeEntrada(rolDto);
            if (!respuestaValidarEntrada.Valido)
            {
                respuesta.Valido = respuestaValidarEntrada.Valido;
                respuesta.Mensaje = respuestaValidarEntrada.Mensaje;
                return respuesta;
            }

            var yaExisteNombre = await _rolRepository.AsQueryable()
                                                     .AnyAsync(r => r.Nombre.ToLower().Equals(rolDto.Nombre!.ToLower()));
            if (yaExisteNombre)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.YaExisteRegistro;
                return respuesta;
            }

            try
            {
                var rol = _mapper.Map<Rol>(rolDto);

                await _rolRepository.AddAsync(rol);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<RolDto>(rol);
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

        public async Task<Respuesta<List<RolDto>>> ObtenerRoles()
        {
            var respuesta = new Respuesta<List<RolDto>>();
            try
            {
                var roles = await _rolRepository.AsQueryable().ToListAsync();

                if (roles.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                    return respuesta;
                }

                var rolesDto = new List<RolDto>();

                foreach (var rol in roles)
                {
                    rolesDto.Add(_mapper.Map<RolDto>(rol));
                }

                respuesta.Datos = rolesDto;
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

        public async Task<Respuesta<RolDto>> ObtenerRolPorId(int rolId)
        {
            var respuesta = new Respuesta<RolDto>();
            try
            {
                var rol = await _rolRepository.AsQueryable().FirstOrDefaultAsync(r => r.RolId == rolId);

                if (rol == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var rolDto = _mapper.Map<RolDto>(rol);

                respuesta.Datos = rolDto;
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

        public async Task<Respuesta<RolDto>> ActualizarRol(RolDto rol)
        {
            var respuesta = new Respuesta<RolDto>();
            try
            {
                var rolEncontrado = await _rolRepository.AsQueryable().FirstOrDefaultAsync(r => r.RolId == rol.RolId);

                if (rolEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Rol no existe");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(rol, rolEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = Mensajes.EntidadGuardada;
                respuesta.Datos = _mapper.Map<RolDto>(rolEncontrado);
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

        public async Task<Respuesta<bool>> EstadoRol(int rolId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var rolEncontrado = await _rolRepository.AsQueryable().FirstOrDefaultAsync(r => r.RolId == rolId);

                if (rolEncontrado == null)
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

                rolEncontrado.Activo = estado;
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
