using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class SucursalService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public SucursalService(SistemaTransporteDrContext sistemaTransporteDrContext, IMapper mapper)
        {
            _context = sistemaTransporteDrContext;
            _mapper = mapper;

        }

        public async Task<Respuesta<SucursalDto>> CrearSucursal(int usuarioActualizaId, SucursalDto sucursalDto)
        {

            var respuesta = new Respuesta<SucursalDto>();

            if (sucursalDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió una sucursal valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(sucursalDto.Nombre) || string.IsNullOrWhiteSpace(sucursalDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del la sucursal es requerido.";
                return respuesta;
            }
            if (await _context.Roles.AnyAsync(c => c.Nombre.ToLower() == sucursalDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe una sucursal con este nombre.";
                return respuesta;
            }

            var sucursal = _mapper.Map<Sucursal>(sucursalDto);
            sucursal.UsuarioCrea = usuarioActualizaId;

            try
            {

                await _context.Sucursales.AddAsync(sucursal);
                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<SucursalDto>(sucursal); ;
                respuesta.Mensaje = "Sucursal cread con éxito.";
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

        public async Task<Respuesta<List<SucursalDto>>> ObtenerSucursales()
        {
            var respuesta = new Respuesta<List<SucursalDto>>();
            try
            {
                var sucursales = await _context.Sucursales.ToListAsync();

                if (sucursales.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Sucursales";
                    return respuesta;
                }

                var sucursalesDto = new List<SucursalDto>();

                foreach (var sucursal in sucursales)
                {
                    sucursalesDto.Add(_mapper.Map<SucursalDto>(sucursal));
                }

                respuesta.Datos = sucursalesDto;

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

        public async Task<Respuesta<SucursalDto>> ObtenerSucursalPorId(int sucursalId)
        {
            var respuesta = new Respuesta<SucursalDto>();
            try
            {
                var sucursal = await _context.Sucursales.FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                if (sucursal == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Rol no encontrado.";
                }
                var rolDto = _mapper.Map<SucursalDto>(sucursal);

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

        public async Task<Respuesta<SucursalDto>> ActualizarSucursal(int usuarioActualizaId, SucursalDto sucursalDto)
        {
            var respuesta = new Respuesta<SucursalDto>();
            try
            {
                var sucursalEncontrada = await _context.Sucursales.FirstOrDefaultAsync(s => s.SucursalId == sucursalDto.SucursalId);

                if (sucursalEncontrada == null)
                {
                    respuesta.Mensaje = "Sucursal no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(sucursalDto, sucursalEncontrada);
                sucursalEncontrada.UsuarioActualiza = usuarioActualizaId;
                sucursalEncontrada.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();
                respuesta.Mensaje = "Rol actualizado con exito";
                respuesta.Datos = sucursalDto;
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

        public async Task<Respuesta<bool>> EstadoSucursal(int sucursalId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var SucursalEncontrada = await _context.Sucursales.FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                if (SucursalEncontrada == null)
                {
                    respuesta.Mensaje = "Sucursal no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Sucursal ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Sucursal ha sido inactivado.";
                }

                SucursalEncontrada.Activo = estado;
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
