using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class SucursalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Sucursal> _sucursalRepository;
        private readonly IMapper _mapper;
        private readonly ManejoDistanciasService _manejoDistanciasService;
        private readonly SucursalDomain _sucursalDomain;

        public SucursalService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, ManejoDistanciasService manejoDistanciasService, SucursalDomain sucursalDomain)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _sucursalRepository = _unitOfWork.Repository<Sucursal>();
            _mapper = mapper;
            _manejoDistanciasService = manejoDistanciasService;
            _sucursalDomain = sucursalDomain;

        }

        public async Task<Respuesta<bool>> AgregarRelacionEntreSucursalColaborador(int sucursalId, int colaboradorId)
        {
            var respuesta = new Respuesta<bool>();

            //validad si ya existe una relacion
            if (colaboradorId <= 0 || sucursalId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Se debe de recibir una sucursal y colaborador valido.";
                return respuesta;
            }
            try
            {
                var colaborador = await _unitOfWork.Repository<Colaborador>()
                                                    .AsQueryable().AsNoTracking()
                                                    .FirstOrDefaultAsync(c => c.ColaboradorId == colaboradorId);
                var sucursal = await _unitOfWork.Repository<Sucursal>()
                                                .AsQueryable().AsNoTracking()
                                                .FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                if (colaborador == null || sucursal == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "El colaborador o la sucursal no existe en la base de datos.";
                    return respuesta;
                }
                if (await _unitOfWork.Repository<SucursalColaborador>().AsQueryable().AsNoTracking().AnyAsync(sc => sc.ColaboradorId == colaborador.ColaboradorId && sc.SucursalId == sucursal.SucursalId))
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Ya existe la relacion entre esta sucursal y este colaborador.";
                    return respuesta;
                }

                var kmsDeDistancia = await _manejoDistanciasService.ObtenerDistanciaEntreSucursalColaborador(sucursal.Latitud, sucursal.Longitud, colaborador.Latitud, colaborador.Longitud);

                var sucursalColaborador = new SucursalColaborador()
                {
                    ColaboradorId = colaboradorId,
                    SucursalId = sucursalId,
                    DistanciaKm = kmsDeDistancia
                };

                await _unitOfWork.Repository<SucursalColaborador>().AddAsync(sucursalColaborador);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Mensaje = "Relacion entre colaborador y sucursal guardada con éxito.";
                respuesta.Datos = true;

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

        public async Task<Respuesta<SucursalDto>> CrearSucursal(int usuarioActualizaId, SucursalDto sucursalDto)
        {

            var respuesta = new Respuesta<SucursalDto>();

            var respuestaValidarDatosSucursal = _sucursalDomain.ValidarCreacionSucursal(sucursalDto);
            if (!respuestaValidarDatosSucursal.Valido)
            {
                respuesta.Valido = respuestaValidarDatosSucursal.Valido;
                respuesta.Mensaje = respuestaValidarDatosSucursal.Mensaje;
                return respuesta;
            }

            if (await _sucursalRepository.AsQueryable().AnyAsync(c => c.Nombre.ToLower() == sucursalDto.Nombre.ToLower()))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Ya existe una sucursal con este nombre.";
                return respuesta;
            }

            var sucursal = _mapper.Map<Sucursal>(sucursalDto);
            sucursal.UsuarioCrea = usuarioActualizaId;

            var convertirCordenadasADireccion = await  _manejoDistanciasService.ObtenerDireccionDesdeCordenadas(sucursalDto.Latitud, sucursalDto.Longitud);
            sucursal.Direccion = convertirCordenadasADireccion;

            try
            {

                await _sucursalRepository.AddAsync(sucursal);
                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<SucursalDto>(sucursal); ;
                respuesta.Mensaje = "Sucursal creada con éxito.";
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<List<SucursalDto>>> ObtenerSucursales()
        {
            var respuesta = new Respuesta<List<SucursalDto>>();
            try
            {
                var sucursales = await _sucursalRepository.AsQueryable().ToListAsync();

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
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<SucursalDto>> ObtenerSucursalPorId(int sucursalId)
        {
            var respuesta = new Respuesta<SucursalDto>();
            try
            {
                if (sucursalId <= 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Id de Sucursal no válido.";
                    return respuesta;
                }
                var sucursal = await _sucursalRepository.AsQueryable().FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                if (sucursal == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Sucursal no encontrada.";
                }
                var rolDto = _mapper.Map<SucursalDto>(sucursal);

                respuesta.Datos = rolDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al guardar en la base de datos.";
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<SucursalDto>> ActualizarSucursal(int usuarioActualizaId, SucursalDto sucursalDto)
        {
            var respuesta = new Respuesta<SucursalDto>();
            try
            {
                var sucursalEncontrada = await _sucursalRepository.AsQueryable().FirstOrDefaultAsync(s => s.SucursalId == sucursalDto.SucursalId);

                if (sucursalEncontrada == null)
                {
                    respuesta.Mensaje = "Sucursal no existe";
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(sucursalDto, sucursalEncontrada);
                sucursalEncontrada.UsuarioActualiza = usuarioActualizaId;
                sucursalEncontrada.FechaActualizacion = DateTime.Now;

                var convertirCordenadasADireccion = await _manejoDistanciasService.ObtenerDireccionDesdeCordenadas(sucursalDto.Latitud, sucursalDto.Longitud);
                sucursalEncontrada.Direccion = convertirCordenadasADireccion;

                await _unitOfWork.SaveChangesAsync();
                respuesta.Mensaje = "Sucursal actualizado con exito";
                respuesta.Datos = sucursalDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al actualizar en la base de datos.";
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<bool>> EstadoSucursal(int sucursalId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var SucursalEncontrada = await _sucursalRepository.AsQueryable().FirstOrDefaultAsync(s => s.SucursalId == sucursalId);

                if (SucursalEncontrada == null)
                {
                    respuesta.Mensaje = "Sucursal no existe";
                    respuesta.Datos = false;
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
                respuesta.Datos = true;

                await _unitOfWork.SaveChangesAsync();


            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Error al actualizar en la base de datos.";
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = "Ocurrió un error inesperado.";
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

    }
}
