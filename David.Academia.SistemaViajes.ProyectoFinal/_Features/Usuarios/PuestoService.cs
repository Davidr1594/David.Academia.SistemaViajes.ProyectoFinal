using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios
{
    public class PuestoService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public PuestoService (SistemaTransporteDrContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Respuesta<PuestoDto>> CrearPuesto(PuestoDto puestoDto)
        {
            var respuesta = new Respuesta<PuestoDto>();

            if (puestoDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un puesto válido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(puestoDto.Nombre) || string.IsNullOrWhiteSpace(puestoDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del puesto es requerido.";
                return respuesta;
            }
            if (await _context.Puestos.AnyAsync(p => p.Nombre.ToLower() == puestoDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un puesto con este nombre.";
                return respuesta;
            }

            try
            {
                var puesto = _mapper.Map<Puesto>(puestoDto);

                await _context.Puestos.AddAsync(puesto);
                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<PuestoDto>(puesto);
                respuesta.Mensaje = "Puesto creado con éxito.";
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

        public async Task<Respuesta<List<PuestoDto>>> ObtenerPuestos()
        {
            var respuesta = new Respuesta<List<PuestoDto>>();
            try
            {
                var puestos = await _context.Puestos.ToListAsync();

                if (puestos.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay Puestos";
                    return respuesta;
                }

                var puestosDto = new List<PuestoDto>();

                foreach (var puesto in puestos)
                {
                    puestosDto.Add(_mapper.Map<PuestoDto>(puesto));
                }

                respuesta.Datos = puestosDto;
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

        public async Task<Respuesta<PuestoDto>> ObtenerPuestoPorId(int puestoId)
        {
            var respuesta = new Respuesta<PuestoDto>();
            try
            {
                var puesto = await _context.Puestos.FirstOrDefaultAsync(p => p.PuestoId == puestoId);

                if (puesto == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Puesto no encontrado.";
                }
                var puestoDto = _mapper.Map<PuestoDto>(puesto);

                respuesta.Datos = puestoDto;
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

        public async Task<Respuesta<PuestoDto>> ActualizarPuesto(PuestoDto puesto)
        {
            var respuesta = new Respuesta<PuestoDto>();
            try
            {
                var puestoEncontrado = await _context.Puestos.FirstOrDefaultAsync(p => p.PuestoId == puesto.PuestoId);

                if (puestoEncontrado == null)
                {
                    respuesta.Mensaje = "Puesto no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(puesto, puestoEncontrado);

                await _context.SaveChangesAsync();
                respuesta.Mensaje = "Puesto actualizado con éxito";
                respuesta.Datos = _mapper.Map<PuestoDto>(puestoEncontrado);
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

        public async Task<Respuesta<bool>> EstadoPuesto(int puestoId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var puestoEncontrado = await _context.Puestos.FirstOrDefaultAsync(p => p.PuestoId == puestoId);

                if (puestoEncontrado == null)
                {
                    respuesta.Mensaje = "Puesto no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Puesto ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Puesto ha sido inactivado.";
                }

                puestoEncontrado.Activo = estado;
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
