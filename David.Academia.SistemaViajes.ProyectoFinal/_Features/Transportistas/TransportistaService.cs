using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas
{
    public class TransportistaService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public TransportistaService (SistemaTransporteDrContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Respuesta<TransportistaDto>> CrearTransportista(int usuarioCrea, TransportistaDto transportistaDto)
        {

            var respuesta = new Respuesta<TransportistaDto>();

            if (transportistaDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un usuario valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(transportistaDto.Nombre) || string.IsNullOrWhiteSpace(transportistaDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del usuario es requerido.";
                return respuesta;
            }
            if (await _context.Roles.AnyAsync(t => t.Nombre.ToLower() == transportistaDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un usuario con este nombre.";
                return respuesta;
            }

            var transportistaACrear = _mapper.Map<Transportista>(transportistaDto);
            transportistaACrear.UsuarioCrea = usuarioCrea;
           
            try
            {

                await _context.Transportistas.AddAsync(transportistaACrear);
                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<TransportistaDto>(transportistaACrear);
                respuesta.Mensaje = ("Transportista creado con éxito.");
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

        public async Task<Respuesta<List<TransportistaDto>>> ObtenerTransportistas()
        {
            var respuesta = new Respuesta<List<TransportistaDto>>();
            try
            {
                var transportistas = await _context.Transportistas.AsQueryable().AsNoTracking().ToListAsync();

                if (transportistas.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay transportistas";
                    return respuesta;
                }

                var transportistasDto = new List<TransportistaDto>();

                foreach (var transportista in transportistas)
                {
                    transportistasDto.Add(_mapper.Map<TransportistaDto>(transportista));
                }

                respuesta.Datos = transportistasDto;
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

        public async Task<Respuesta<TransportistaDto>> ObtenerTransportistaPorId(int transportistaId)
        {
            var respuesta = new Respuesta<TransportistaDto>();
            try
            {
                var transportista = await _context.Transportistas.FirstOrDefaultAsync(t => t.TransportistaId == transportistaId);

                if (transportista == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Transportista no encontrado.";
                }
                var transportistaDto = _mapper.Map<TransportistaDto>(transportista);

                respuesta.Datos = transportistaDto;
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

        public async Task<Respuesta<TransportistaDto>> ActualizarTransportista(int usuarioActualizaId, TransportistaDto transportistaDto)
        {
            var respuesta = new Respuesta<TransportistaDto>();
            try
            {
                var transportistaEncontrado = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == transportistaDto.TransportistaId);

                if (transportistaEncontrado == null)
                {
                    respuesta.Mensaje = "Usuario no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(transportistaDto, transportistaEncontrado);
                transportistaEncontrado.UsuarioActualiza = usuarioActualizaId;

                await _context.SaveChangesAsync();

                respuesta.Datos = transportistaDto;
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

        public async Task<Respuesta<bool>> EstadoTransportista(int transportistaId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var transportistaEncontrado = await _context.Transportistas.FirstOrDefaultAsync(t => t.TransportistaId == transportistaId);

                if (transportistaEncontrado == null)
                {
                    respuesta.Mensaje = "Transportista no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Transportista ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Transportista ha sido inactivado.";
                }

                transportistaEncontrado.Activo = estado;
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
