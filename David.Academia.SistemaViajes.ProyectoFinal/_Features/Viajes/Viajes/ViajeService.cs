using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common0;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Enum;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes
{
    public class ViajeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Viaje> _viajeRepository;
        private readonly ViajeDomain _viajeDomain;
        private readonly ManejoDistanciasService _manejoDistanciasService;
        private readonly IMapper _mapper;
        private readonly decimal _maximoKms;

        public ViajeService(UnitOfWorkBuilder unitOfWorkBuilder, IMapper mapper, ViajeDomain viajeDomain, ManejoDistanciasService manejoDistanciasService)
        {
            _unitOfWork = unitOfWorkBuilder.BuildSistemaDeTransporte();
            _viajeRepository = _unitOfWork.Repository<Viaje>();
            _maximoKms = obtenerParametroDeKms();
            _manejoDistanciasService = manejoDistanciasService;

            _mapper = mapper;
            _viajeDomain = viajeDomain;
        }

        public async Task<Respuesta<ViajeDto>> CrearViaje(int usuarioCreaId, ViajeDto viajeDto)
        {
            var respuesta = new Respuesta<ViajeDto>();

            var respuestaValidarDatosRecibido = _viajeDomain.ValidarDatosDeEntradaViaje(viajeDto);

            if (!respuestaValidarDatosRecibido.Valido)
            {
                respuesta.Valido = respuestaValidarDatosRecibido.Valido;
                respuesta.Mensaje = respuestaValidarDatosRecibido.Mensaje;
                return respuesta;
            }

            try
            {

                var sucursal = await ObtenerSucursalAsync(viajeDto.SucursalId);
                var usucarioCrea = await ObtenerUsuarioAsync(usuarioCreaId);
                var transportista = await ObtenerTransportistaAsync(viajeDto.TransportistaId);
                var colaboradoresDetalle = await ObtenerColaboradoresDetalleAsync(viajeDto.ColaboradoresId);
                var colaboradoresEnViaje = await ValidarColaboradoresTieneViajeAsigando(viajeDto.ColaboradoresId);

                var respuestaValidarRespuestasBaseDatos = _viajeDomain.ValidarRespuestasDeBD(sucursal, usucarioCrea, transportista, colaboradoresDetalle, colaboradoresEnViaje);
                if (!respuestaValidarRespuestasBaseDatos.Valido)
                {
                    respuesta.Valido = respuestaValidarRespuestasBaseDatos.Valido;
                    respuesta.Mensaje = respuestaValidarRespuestasBaseDatos.Mensaje;
                    return respuesta;
                }

                var respuestaEsGerente = _viajeDomain.EsGerente(usucarioCrea);
                if (!respuestaEsGerente.Valido)
                {
                    respuesta.Valido = respuestaEsGerente.Valido;
                    respuesta.Mensaje = respuestaEsGerente.Mensaje;

                    return respuesta;
                }

                var ValidarKmsDeColaborador = _viajeDomain.ValidarKmsDeColaborador(colaboradoresDetalle, _maximoKms);
                if (!ValidarKmsDeColaborador.Valido)
                {
                    respuesta.Valido = ValidarKmsDeColaborador.Valido;
                    respuesta.Mensaje = ValidarKmsDeColaborador.Mensaje;
                    return respuesta;
                }

                var respuestaDistanciaTotalViaje = await _manejoDistanciasService.CalcularDistanciaTotalAjustadaAsync(colaboradoresDetalle, 5m);
                if (!respuestaDistanciaTotalViaje.Valido)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = respuestaDistanciaTotalViaje.Mensaje;
                    return respuesta;
                }

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    var totalKmsSinRadio = respuestaDistanciaTotalViaje.Datos;
                    var precioPorKm = transportista.TarifaPorKm;

                    var viajeACrear = _mapper.Map<Viaje>(viajeDto);
                    viajeACrear.UsuarioCrea = usuarioCreaId;
                    viajeACrear.TotalKms = totalKmsSinRadio;
                    viajeACrear.EstadoId = (int)EstadoViajeEnum.Pendiente;
                    viajeACrear.MontoTotal = totalKmsSinRadio * precioPorKm;
                    viajeACrear.Activo = true;

                    await _viajeRepository.AddAsync(viajeACrear);

                    if (!await _unitOfWork.SaveChangesAsync())
                    {
                        respuesta.Valido = false;
                        respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;

                        await _unitOfWork.RollBackAsync();

                        return respuesta;
                    }

                    foreach (var colaborador in colaboradoresDetalle)
                    {
                        var viajeDetalle = new ViajeDetalle()
                        {
                            ViajeId = viajeACrear.ViajeId,
                            ColaboradorId = colaborador.ColaboradorId,
                            DireccionDestino = colaborador.DireccionDestino,
                            Kms = colaborador.DistanciaKms,
                        };

                        await _unitOfWork.Repository<ViajeDetalle>().AddAsync(viajeDetalle);

                    }
                    if (!await _unitOfWork.SaveChangesAsync())
                    {
                        respuesta.Valido = false;
                        respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                        await _unitOfWork.RollBackAsync();
                        return respuesta;
                    }

                    await _unitOfWork.CommitAsync();

                    respuesta.Datos = _mapper.Map<ViajeDto>(viajeACrear);
                    respuesta.Mensaje = Mensajes.EntidadGuardada;
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollBackAsync();
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                    respuesta.DetalleError = ex.Message;
                }

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
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<bool>> AgregarColaboradoresAViaje(int usuarioCrea,int sucursalId, List<int> colaboradoresId)
        {

            var respuesta = new Respuesta<bool>();

            try
            {

                var viaje = await ObtenerViajeAsync(sucursalId);
                var colaboradoresConKms = await ObtenerColaboradoresDetalleAsync(colaboradoresId);
                var colaboradoresYaAsignados = await ObtenerColaboradoresAsignadosAsync(colaboradoresId);
                var colaboradoresValidos = colaboradoresConKms.Where(c => !colaboradoresYaAsignados.Any(ca => ca.ColaboradorId == c.ColaboradorId)).ToList();

                var respuestaValidarDatosDb = _viajeDomain.ValidarRespuestasDeBDAgregarColaboradores(viaje, colaboradoresConKms, colaboradoresValidos);
                if (!respuestaValidarDatosDb.Valido)
                {
                    respuesta.Valido = respuestaValidarDatosDb.Valido;
                    respuesta.Mensaje = respuestaValidarDatosDb.Mensaje;
                    return respuesta;
                }


                var validarDistanciaNuevosColaboradores = await _manejoDistanciasService.ValidarDistanciaEntreColaboradoresExistentesEnViajeAsync(colaboradoresYaAsignados, colaboradoresValidos, 5m);
                if (!validarDistanciaNuevosColaboradores.Valido)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = validarDistanciaNuevosColaboradores.Mensaje;
                    return respuesta;
                }

                var respuestaDistanciaTotalViaje = await _manejoDistanciasService.CalcularDistanciaTotalAjustadaAsync(colaboradoresValidos, 5m);             
                if (!respuestaDistanciaTotalViaje.Valido)
                {
                    respuesta.Valido = respuestaDistanciaTotalViaje.Valido;
                    respuesta.Mensaje = respuestaDistanciaTotalViaje.Mensaje;
                    return respuesta;
                }

                var totalKmsViaje = respuestaDistanciaTotalViaje.Datos;

                var respuestaValidarKmsViaje = _viajeDomain.ValidarKmsDeViajeAlAgregarColaborador(totalKmsViaje, viaje, _maximoKms);

                if (!respuestaValidarKmsViaje.Valido)
                {
                    respuesta.Valido = respuestaValidarKmsViaje.Valido;
                    respuesta.Mensaje = respuestaValidarKmsViaje.Mensaje;
                    return respuesta;
                }
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    
                    foreach (var colaborador in colaboradoresValidos)
                    {
                        var viajeDetalle = new ViajeDetalle()
                        {
                            ViajeId = viaje.ViajeId,
                            ColaboradorId = colaborador.ColaboradorId,
                            DireccionDestino = colaborador.DireccionDestino,
                            Kms = colaborador.DistanciaKms,
                        };

                            await _unitOfWork.Repository<ViajeDetalle>().AddAsync(viajeDetalle);
                    }

                        if (!await _unitOfWork.SaveChangesAsync())
                        {
                            await _unitOfWork.RollBackAsync();
                            respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                            respuesta.Valido = false;
                            respuesta.Datos = false;

                            return respuesta;
                        }

                    viaje.TotalKms += totalKmsViaje;
                    _unitOfWork.Repository<Viaje>().Update(viaje);

                    if (!await _unitOfWork.SaveChangesAsync())
                    {
                        await _unitOfWork.RollBackAsync();
                        respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                        respuesta.Valido = false;
                        respuesta.Datos = false;

                        return respuesta;
                    }
                    respuesta.Datos = true;
                    respuesta.Mensaje = Mensajes.EntidadGuardada;
                    await _unitOfWork.CommitAsync();

                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollBackAsync();
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.ErrorExcepcion;
                    respuesta.DetalleError = ex.Message;
                }
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
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            return respuesta;
        }
        public async Task<Respuesta<bool>> ActualizadoEstadoViaje(int usuarioActualiza,int sucursalId, int viajeId, int estadoId)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var viajeEncontrado = await _viajeRepository.AsQueryable().FirstOrDefaultAsync(v => v.ViajeId == viajeId && v.Activo == true);

                if (viajeEncontrado == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Viaje no existe";
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estadoId == (int)EstadoViajeEnum.Completado)
                {
                    viajeEncontrado.EstadoId = estadoId;
                    respuesta.Mensaje = "Viaje ha sido aprobado";
                }
                if (estadoId == (int)EstadoViajeEnum.Pendiente)
                {
                    viajeEncontrado.EstadoId = estadoId;
                    respuesta.Mensaje = "Viaje ha sido cambiado a pendiente";
                }
                if (estadoId == (int)EstadoViajeEnum.Pendiente)
                {
                    viajeEncontrado.EstadoId = estadoId;
                    respuesta.Mensaje = "Viaje ha sido cancelado";
                }


                respuesta.Valido = true;
                respuesta.Datos = true;
                viajeEncontrado.FechaActualiza = DateTime.Now;
                viajeEncontrado.UsuarioActualiza = usuarioActualiza;
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
        public async Task<Respuesta<List<ViajeDto>>> ObtenerViajes()
        {
            var respuesta = new Respuesta<List<ViajeDto>>();
            try
            {
                var viajes = await _viajeRepository.AsQueryable().ToListAsync();

                if (viajes.Count == 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
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
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError =Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }
        public async Task<Respuesta<ViajeDto>> ObtenerViajePorId(int viajeId)
        {
            var respuesta = new Respuesta<ViajeDto>();
            try
            {
                var viaje = await _viajeRepository.AsQueryable().FirstOrDefaultAsync(v => v.ViajeId == viajeId);

                if (viaje == null)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = Mensajes.NoHayEntidades;
                }
                var viajeDto = _mapper.Map<ViajeDto>(viaje);

                respuesta.Datos = viajeDto;
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }
            return respuesta;
        }

        public async Task<Respuesta<ViajeDto>> ActualizarViaje(int usuarioActualizaId, ViajeDto viaje)
        {
            var respuesta = new Respuesta<ViajeDto>();
            try
            {
                var viajeEncontrado = await _viajeRepository.AsQueryable().FirstOrDefaultAsync(v => v.ViajeId == viaje.ViajeId);

                if (viajeEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste,"Viaje");
                    respuesta.Valido = false;
                    return respuesta;
                }

                _mapper.Map(viaje, viajeEncontrado);
                viajeEncontrado.UsuarioActualiza = usuarioActualizaId;
                viajeEncontrado.FechaActualiza = DateTime.Now;

                await _unitOfWork.SaveChangesAsync();

                respuesta.Datos = _mapper.Map<ViajeDto>(viajeEncontrado);
            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }

        public async Task<Respuesta<bool>> EstadoViajeActivo(int viajeId, bool estado)
        {
            var respuesta = new Respuesta<bool>();
            try
            {
                var viajeEncontrado = await _viajeRepository.AsQueryable().FirstOrDefaultAsync(v => v.ViajeId == viajeId);

                if (viajeEncontrado == null)
                {
                    respuesta.Mensaje = string.Format(Mensajes.EntidadNoExiste, "Viaje");
                    respuesta.Datos = false;
                    return respuesta;
                }
                if (estado)
                {
                    respuesta.Mensaje = Mensajes.EntidadActivada;
                }
                else if (!estado)
                {
                    respuesta.Mensaje = Mensajes.EntidadInactivada;
                }

                viajeEncontrado.Activo = estado;
                respuesta.Datos = true;
                await _unitOfWork.SaveChangesAsync();

            }
            catch (DbUpdateException ex)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = Mensajes.ErrorGuardarEntidad;
                respuesta.DetalleError = ex.InnerException?.Message ?? ex.Message;
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Valido = false;
                respuesta.DetalleError = Mensajes.ErrorExcepcion;
                respuesta.Mensaje = ex.Message;
                return respuesta;
            }

            return respuesta;
        }




        //metodos privados

        private async Task<Sucursal> ObtenerSucursalAsync(int sucursalId)
        {
            return await _unitOfWork.Repository<Sucursal>()
                                                .AsQueryable()
                                                .FirstOrDefaultAsync(s => s.SucursalId == sucursalId && s.Activo == true) ?? new Sucursal();
        }
        private async Task<Usuario> ObtenerUsuarioAsync(int usuarioCreaId)
        {
            return await _unitOfWork.Repository<Usuario>()
                                                    .AsQueryable()
                                                    .FirstOrDefaultAsync(u => u.UsuarioId == usuarioCreaId && u.Activo == true) ?? new Usuario();
        }
        private async Task<Transportista> ObtenerTransportistaAsync(int transporstistaId)
        {
            return await _unitOfWork.Repository<Transportista>()
                                                    .AsQueryable()
                                                    .FirstOrDefaultAsync(t => t.TransportistaId == transporstistaId && t.Activo == true) ?? new Transportista();
        }
        private async Task<List<ColaboradorConKmsDto>> ObtenerColaboradoresDetalleAsync(List<int> colaboradoresId)
        {
            return await (from c in _unitOfWork.Repository<Colaborador>().AsQueryable().AsNoTracking()
                          join cs in _unitOfWork.Repository<SucursalColaborador>().AsQueryable() on c.ColaboradorId equals cs.ColaboradorId
                          where colaboradoresId.Contains(c.ColaboradorId)
                          select new ColaboradorConKmsDto
                          {
                              ColaboradorId = c.ColaboradorId,
                              DireccionDestino = c.Direccion,
                              Nombre = c.Nombre,
                              DistanciaKms = cs.DistanciaKm,
                              latitud = c.Latitud,
                              longitud = c.Longitud
                              
                          }).ToListAsync() ?? new List<ColaboradorConKmsDto>();

        }
        private async Task<List<int>> ValidarColaboradoresTieneViajeAsigando(List<int> colaboradoresId)
        {
            return await _unitOfWork.Repository<ViajeDetalle>()
                                                            .AsQueryable().AsNoTracking()
                                                            .Where(vd => colaboradoresId.Contains(vd.ColaboradorId) && vd.Viaje.FechaCreacion.Date == DateTime.Now.Date)
                                                            .Select(vd => vd.ColaboradorId)
                                                            .Distinct()
                                                            .ToListAsync();

        }
        private async Task<Viaje> ObtenerViajeAsync(int sucursalId)
        {
            return await _unitOfWork.Repository<Viaje>()
                                        .AsQueryable().AsNoTracking()
                                        .FirstOrDefaultAsync(v => v.SucursalId == sucursalId && v.FechaCreacion.Date == DateTime.Now.Date && v.EstadoId == (int)EstadoViajeEnum.Pendiente) ?? new Viaje();

        }
        private async Task<List<ColaboradorConKmsDto>> ObtenerColaboradoresAsignadosAsync(List<int> colaboradoresId)
        {
            return await (from v in _unitOfWork.Repository<Viaje>().AsQueryable().AsNoTracking()
                          join vd in _unitOfWork.Repository<ViajeDetalle>().AsQueryable().AsNoTracking()
                          on v.ViajeId equals vd.ViajeId
                          join c in _unitOfWork.Repository<Colaborador>().AsQueryable().AsNoTracking()
                          on vd.ColaboradorId equals c.ColaboradorId
                          where v.FechaCreacion.Date == DateTime.Now.Date
                          select new ColaboradorConKmsDto
                          {
                              ColaboradorId = vd.ColaboradorId,
                              Nombre = vd.Colaborador.Nombre,
                              DireccionDestino = vd.DireccionDestino,
                              DistanciaKms = vd.Kms,
                              latitud = c.Latitud,
                              longitud = c.Longitud
                          }).ToListAsync();

        }
        private decimal obtenerParametroDeKms()
        {
           return _unitOfWork.Repository<ParametroSistema>().AsQueryable().AsNoTracking().Where(ps => ps.RegistroId == 1 && ps.Activo == true).Select(ps => (decimal?)ps.Valor).FirstOrDefault() ?? 0m;
        }
    }
}
