using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes.Enum;
using David.Academia.SistemaViajes.ProyectoFinal._Infrastructure;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes
{
    public class ReportesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ReporteDomain _reporteDomain;

        public ReportesService (UnitOfWorkBuilder unitOfWork, ReporteDomain reporteDomain)
        {
            _unitOfWork = unitOfWork.BuildSistemaDeTransporte();
            _reporteDomain = reporteDomain;
        }

        public async Task<Respuesta<ReporteViajeResumenDto>> TraerReporteDeViajesPorFechas(int transportistaId, DateTime fechaInicio, DateTime fechaFin)
        { 
            var respuesta = new Respuesta<ReporteViajeResumenDto> ();

            var respuestaValidacionDatosEntrada = _reporteDomain.validarDatosEntrada(transportistaId, fechaInicio, fechaFin);

            if (!respuestaValidacionDatosEntrada.Valido)
            {
                respuesta.Valido = respuestaValidacionDatosEntrada.Valido;
                respuesta.Mensaje = respuestaValidacionDatosEntrada.Mensaje;
                return respuesta;
            }

            var transportista = await _unitOfWork.Repository<Transportista>().AsQueryable().AsNoTracking().FirstOrDefaultAsync(t => t.TransportistaId == transportistaId && t.Activo == true);

            if (transportista == null)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Transportista no existe o esta inactivo.";
                return respuesta;
            }
            try
            {
                var reporte = await (from v in _unitOfWork.Repository<Viaje>().AsQueryable().AsNoTracking()
                                     join t in _unitOfWork.Repository<Transportista>().AsQueryable().AsNoTracking() on v.TransportistaId equals t.TransportistaId
                                     join s in _unitOfWork.Repository<Sucursal>().AsQueryable().AsNoTracking() on v.SucursalId equals s.SucursalId
                                     join m in _unitOfWork.Repository<Moneda>().AsQueryable().AsNoTracking() on v.MonedaId equals m.MonedaId
                                     where v.FechaCreacion.Date >= fechaInicio.Date && v.FechaCreacion <= fechaFin.Date
                                     select new ReporteViajeDto()
                                     {
                                         ViajeId = v.ViajeId,
                                         FechaCreacion = v.FechaCreacion,
                                         SucursalNombre = s.Nombre,
                                         TransportistaNombre = t.Nombre,
                                         HoraSalida = v.HoraSalida,
                                         TotalKms = v.TotalKms,
                                         MontoTotal = v.MontoTotal,
                                         Moneda = m.CodigoIso

                                     }).ToListAsync();
                if (reporte.Count <= 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Error al obtener el reporte";
                    return respuesta;
                }

                decimal totalMonto = reporte.Sum(r => r.MontoTotal);
                decimal totalKms = reporte.Sum(r => r.TotalKms);

                var resumen = new ReporteViajeResumenDto
                {
                    TotalMonto = totalMonto,
                    TotalKms = totalKms,
                    Viajes = reporte
                };
                respuesta.Valido = true;
                respuesta.Datos = resumen;
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

        public async Task<Respuesta<List<ViajeDetalleReporteDto>>> traerReportePorColaborador(int viajeId)
        {
            var respuesta = new Respuesta<List<ViajeDetalleReporteDto>>();

            if (viajeId <= 0 )
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Id de viaje no valido";
                return respuesta;
            }
            if (!await _unitOfWork.Repository<Viaje>().AsQueryable().AsNoTracking().AnyAsync(v => v.ViajeId == viajeId && v.Activo == true))
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "No se encuentra el viaje o no esta activo.";
                return respuesta;
            }

            try
            {
                var reporte = await (from v in _unitOfWork.Repository<Viaje>().AsQueryable().AsNoTracking()
                                     join vd in _unitOfWork.Repository<ViajeDetalle>().AsQueryable().AsNoTracking() on v.ViajeId equals vd.ViajeId
                                     join s in _unitOfWork.Repository<Sucursal>().AsQueryable().AsNoTracking() on v.SucursalId equals s.SucursalId
                                     join c in _unitOfWork.Repository<Colaborador>().AsQueryable().AsNoTracking() on vd.ColaboradorId equals c.ColaboradorId
                                     where v.ViajeId == viajeId
                                     select new ViajeDetalleReporteDto()
                                     {
                                         ViajeDetalleId = vd.ViajeDetalleId,
                                         ViajeId = v.ViajeId,
                                         Sucursal = s.Nombre,
                                         NombreColaborador = c.Nombre,
                                         Kms = v.TotalKms,
                                         DireccionDestino = c.Direccion,
                                         FechaViaje = v.FechaCreacion

                                     }).ToListAsync();

                if (reporte.Count <= 0)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "Error al obtener el reporte";
                    return respuesta;
                }

                respuesta.Valido = true;
                respuesta.Datos = reporte;
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
    }
}

public class ReporteViajeResumenDto
    {
        public decimal TotalMonto { get; set; }
        public decimal TotalKms { get; set; }
        public List<ReporteViajeDto> Viajes { get; set; }
    }


