using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes
{
    public class ViajeService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public ViajeService (SistemaTransporteDrContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Respuesta<ViajeDto>> CrearViaje(int usuarioCreaId, ViajeDto viajeDto)
        {
            var respuesta = new Respuesta<ViajeDto>();


            var viajeACrear = _mapper.Map<Viaje>(viajeDto);
            viajeACrear.UsuarioCrea = usuarioCreaId;

            try
            {
                await _context.Viajes.AddAsync(viajeACrear);
                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ViajeDto>(viajeACrear);
                respuesta.Mensaje = "Viaje creado con éxito.";
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

        public async Task<Respuesta<List<ViajeDto>>> ObtenerViajes()
        {
            var respuesta = new Respuesta<List<ViajeDto>>();
            try
            {
                var viajes = await _context.Viajes.ToListAsync();

                if (viajes.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay viajes.";
                    return respuesta;
                }

                var viajesDto = new List<ViajeDto>();

                foreach (var viaje in viajes)
                {
                    viajesDto.Add(_mapper.Map<ViajeDto>(viaje));
                }

                respuesta.Datos = viajesDto;
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

        public async Task<Respuesta<ViajeDto>> ObtenerViajePorId(int viajeId)
        {
            var respuesta = new Respuesta<ViajeDto>();
            try
            {
                var viaje = await _context.Viajes.FirstOrDefaultAsync(v => v.ViajeId == viajeId);

                if (viaje == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Viaje no encontrado.";
                }
                var viajeDto = _mapper.Map<ViajeDto>(viaje);

                respuesta.Datos = viajeDto;
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

        public async Task<Respuesta<ViajeDto>> ActualizarViaje(int usuarioActualizaId, ViajeDto viaje)
        {
            var respuesta = new Respuesta<ViajeDto>();
            try
            {
                var viajeEncontrado = await _context.Viajes.FirstOrDefaultAsync(v => v.ViajeId == viaje.ViajeId);

                if (viajeEncontrado == null)
                {
                    respuesta.Mensaje = "Viaje no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(viaje, viajeEncontrado);
                viajeEncontrado.UsuarioActualiza = usuarioActualizaId;
                viajeEncontrado.FechaActualiza = DateTime.Now;

                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ViajeDto>(viajeEncontrado);
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

        public async Task<Respuesta<bool>> EstadoViaje(int viajeId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var viajeEncontrado = await _context.Viajes.FirstOrDefaultAsync(v => v.ViajeId == viajeId);

                if (viajeEncontrado == null)
                {
                    respuesta.Mensaje = "Viaje no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Viaje ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Viaje ha sido inactivado.";
                }

                viajeEncontrado.Activo = estado;
                respuesta.Datos = true;
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
