using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class CiudadService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public CiudadService(SistemaTransporteDrContext context, IMapper mapper)
        { 
            _context = context;
            _mapper = mapper;
        }

        public async Task<Respuesta<CiudadDto>> CrearCiudad(CiudadDto ciudadDto)
        {
            var respuesta = new Respuesta<CiudadDto>();

            if (ciudadDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió una ciudad valida.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(ciudadDto.Nombre) || string.IsNullOrWhiteSpace(ciudadDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre de la ciudad es requerida.";
                return respuesta;
            }
            if (await _context.Ciudades.AnyAsync(c => c.Nombre.ToLower() == ciudadDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe una ciudad con este nombre.";
                return respuesta;
            }

            try
            {
                var ciudad = _mapper.Map<Ciudad>(ciudadDto);

                await _context.Ciudades.AddAsync(ciudad);
                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<CiudadDto>(ciudad);
                respuesta.Mensaje = "Ciudad creado con éxito.";
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

        public async Task<Respuesta<List<CiudadDto>>> ObtenerCiudades()
        {
            var respuesta = new Respuesta<List<CiudadDto>>();
            try
            {
                var ciudades = await _context.Ciudades.ToListAsync();

                if (ciudades.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Ciudades";
                    return respuesta;
                }

                var ciudadesDto = new List<CiudadDto>();

                foreach (var ciudad in ciudades)
                {
                    ciudadesDto.Add(_mapper.Map<CiudadDto>(ciudad));
                }

                respuesta.Datos = ciudadesDto;

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

        public async Task<Respuesta<CiudadDto>> ObtenerCiudadPorId(int ciudadId)
        {
            var respuesta = new Respuesta<CiudadDto>();
            try
            {
                var ciudad = await _context.Ciudades.FirstOrDefaultAsync(c => c.CiudadId == ciudadId);

                if (ciudad == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Ciudad no encontrado.";
                }
                var ciudadDto = _mapper.Map<CiudadDto>(ciudad);

                respuesta.Datos = ciudadDto;
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

        public async Task<Respuesta<CiudadDto>> ActualizarCiudad(CiudadDto ciudad)
        {
            var respuesta = new Respuesta<CiudadDto>();
            try
            {
                var ciudadEncontrada = await _context.Ciudades.FirstOrDefaultAsync(c => c.CiudadId == ciudad.CiudadId);

                if (ciudadEncontrada == null)
                {
                    respuesta.Mensaje = "Rol no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(ciudad, ciudadEncontrada);

                await _context.SaveChangesAsync();
                respuesta.Mensaje = "Ciudad actualizado con exito";
                respuesta.Datos = _mapper.Map<CiudadDto>(ciudadEncontrada);
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

        public async Task<Respuesta<bool>> EstadoCiudad(int ciudadId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var ciudadEncontrada = await _context.Ciudades.FirstOrDefaultAsync(c => c.CiudadId == ciudadId);

                if (ciudadEncontrada == null)
                {
                    respuesta.Mensaje = "Ciudad no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Ciudad ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Ciudad ha sido inactivado.";
                }

                ciudadEncontrada.Activo = estado;
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
