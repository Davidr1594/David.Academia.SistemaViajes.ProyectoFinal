using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios
{
    public class ColaboradorService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public ColaboradorService( SistemaTransporteDrContext context, IMapper mapper)
        { 
            _context = context;
            _mapper = mapper;
        }

        public async Task<Respuesta<ColaboradorDto>> CrearColaborador(int usuarioCreaId, ColaboradorDto colaboradorDto)
        {
            var respuesta = new Respuesta<ColaboradorDto>();


            if (colaboradorDto == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se recibió un colaborador valido.";
                return respuesta;
            }
            if (string.IsNullOrEmpty(colaboradorDto.Nombre) || string.IsNullOrWhiteSpace(colaboradorDto.Nombre))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "El nombre del colaborador es requerido.";
                return respuesta;
            }
            if (await _context.Roles.AnyAsync(c => c.Nombre.ToLower() == colaboradorDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un colaborador con este nombre.";
                return respuesta;
            }

            var colaborador = _mapper.Map<Colaborador>(colaboradorDto);
            colaborador.UsuarioCrea = usuarioCreaId;

            try
            {

                await _context.Colaboradores.AddAsync(colaborador);
                await _context.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ColaboradorDto>(colaborador);
                respuesta.Mensaje = "Colaborador creado con éxito.";
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

        public async Task<Respuesta<List<ColaboradorDto>>> ObtenerColaboradores()
        {
            var respuesta = new Respuesta<List<ColaboradorDto>>();
            try
            {
                var colaboradores = _context.Colaboradores.AsQueryable().ToList();


                if (colaboradores.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "No hay colaboradores";
                    return respuesta;
                }

                var colaboradoresDto = new List<ColaboradorDto>();

                foreach (var colaborador in colaboradores)
                {
                    var colaboradorDto = _mapper.Map<ColaboradorDto>(colaborador);

                    colaboradoresDto.Add(colaboradorDto);

                }

                respuesta.Datos = colaboradoresDto;
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

        public async Task<Respuesta<ColaboradorDto>> ObtenerColaboradorPorId(int colaboradorId)
        {
            var respuesta = new Respuesta<ColaboradorDto>();
            try
            {
                var colaborador = await _context.Colaboradores.FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);

                if (colaborador == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Colaborador no encontrado.";
                }
                var colaboradorDto = _mapper.Map<ColaboradorDto>(colaborador);

                respuesta.Datos = colaboradorDto;
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

        public async Task<Respuesta<ColaboradorDto>> ActualizarColaborador(int usuarioActualizaId, ColaboradorDto colaborador)
        {
            var respuesta = new Respuesta<ColaboradorDto>();
            try
            {
                var colaboradorEncontrado = await _context.Colaboradores.FirstOrDefaultAsync(c => c.ColaboradorId == colaborador.ColaboradorId);

                if (colaboradorEncontrado == null)
                {
                    respuesta.Mensaje = "Colaborador no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(colaborador, colaboradorEncontrado);
                colaboradorEncontrado.UsuarioActualiza = usuarioActualizaId;
                colaboradorEncontrado.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                respuesta.Datos = colaborador;
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


        public async Task<Respuesta<bool>> EstadoColaborador(int colaboradorId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var colaboradorEncontrado = await _context.Colaboradores.FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);

                if (colaboradorEncontrado == null)
                {
                    respuesta.Mensaje = "Colaborador no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = "Colaborador ha sido activado.";
                }
                else if (!estado)
                {
                    respuesta.Mensaje = "Colaborador ha sido inactivado.";
                }

                colaboradorEncontrado.Activo = estado;
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
