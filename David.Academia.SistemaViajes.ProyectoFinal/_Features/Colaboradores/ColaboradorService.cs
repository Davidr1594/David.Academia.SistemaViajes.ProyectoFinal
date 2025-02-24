using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores
{
    public class ColaboradorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Colaborador> _colaboradorRepository;
        private readonly IMapper _mapper;

        public ColaboradorService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _colaboradorRepository = _unitOfWork.Repository<Colaborador>();
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
            if (await _colaboradorRepository.AsQueryable().AnyAsync(c => c.Nombre.ToLower() == colaboradorDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe un colaborador con este nombre.";
                return respuesta;
            }

            var colaborador = _mapper.Map<Colaborador>(colaboradorDto);
            colaborador.UsuarioCrea = usuarioCreaId;

            try
            {

                await _colaboradorRepository.AddAsync(colaborador);
                await _unitOfWork.SaveChangesAsync();

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
                var colaboradores = _colaboradorRepository.AsQueryable().AsQueryable().ToList();


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
                var colaborador = await _colaboradorRepository.AsQueryable().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);

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
                var colaboradorEncontrado = await _colaboradorRepository.AsQueryable().FirstOrDefaultAsync(c => c.ColaboradorId == colaborador.ColaboradorId);

                if (colaboradorEncontrado == null)
                {
                    respuesta.Mensaje = "Colaborador no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(colaborador, colaboradorEncontrado);
                colaboradorEncontrado.UsuarioActualiza = usuarioActualizaId;
                colaboradorEncontrado.FechaActualizacion = DateTime.Now;

                await _unitOfWork.SaveChangesAsync();

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
                var colaboradorEncontrado = await _colaboradorRepository.AsQueryable().FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);

                if (colaboradorEncontrado == null)
                {
                    respuesta.Mensaje = "Colaborador no existe";
                    respuesta.Datos = false;
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
