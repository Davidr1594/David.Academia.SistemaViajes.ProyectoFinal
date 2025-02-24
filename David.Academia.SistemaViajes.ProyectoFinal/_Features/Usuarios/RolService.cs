using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios
{
    public class RolService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Rol> _rolRepository;
        

        public RolService(IMapper mapper, UnitOfWorkBuilder unitOfWorkBuilder)
        {
            _mapper = mapper;
            
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _rolRepository = _unitOfWork.Repository<Rol>();
        }

        public async Task<Respuesta<RolDto>> CrearRol(RolDto rolDto)
        {
            var respuesta = new Respuesta<RolDto>();

            if (rolDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un rol valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(rolDto.Nombre) || string.IsNullOrWhiteSpace(rolDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del rol es requerido.";
                return respuesta;
            }
            if (await _rolRepository.AsQueryable().AnyAsync(r => r.Nombre.ToLower() == rolDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un rol con este nombre.";
                return respuesta;
            }
            
            try
            {
                var rol = _mapper.Map<Rol>(rolDto);

                

                await _rolRepository.AddAsync(rol);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<RolDto>(rol);
                respuesta.Mensaje = "Rol creado con éxito.";
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

        public async Task<Respuesta<List<RolDto>>> ObtenerRoles()
        {
            var respuesta = new Respuesta<List<RolDto>>();
            try
            {
                var roles = await _rolRepository.AsQueryable().ToListAsync();

                if (roles.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Roles";
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
                respuesta.Mensaje = "Error al conectar en la base de datos.";
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

        public async Task<Respuesta<RolDto>> ObtenerRolPorId(int rolId)
        {
            var respuesta = new Respuesta<RolDto>();
            try
            {
                var rol = await _rolRepository.AsQueryable().FirstOrDefaultAsync(r => r.RolId == rolId);

                if (rol == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Rol no encontrado.";
                }
                var rolDto = _mapper.Map<RolDto>(rol);

                respuesta.Datos = rolDto;
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

        public async Task<Respuesta<RolDto>> ActualizarRol(RolDto rol)
        {
            var respuesta = new Respuesta<RolDto>();
            try
            {
                var rolEncontrado = await _rolRepository.AsQueryable().FirstOrDefaultAsync(r => r.RolId == rol.RolId);

                if (rolEncontrado == null)
                {
                    respuesta.Mensaje = "Rol no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(rol, rolEncontrado);

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "Rol actualizado con exito";
                respuesta.Datos = _mapper.Map<RolDto>(rolEncontrado);
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

        public async Task<Respuesta<bool>> EstadoRol(int rolId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var rolEncontrado = await _rolRepository.AsQueryable().FirstOrDefaultAsync(r => r.RolId == rolId);

                if (rolEncontrado == null)
                {
                    respuesta.Mensaje = "Rol no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Rol ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Rol ha sido inactivado."; 
                }

                rolEncontrado.Activo = estado;
                respuesta.Datos = true;

                await _unitOfWork.SaveChangesAsync();

               
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
