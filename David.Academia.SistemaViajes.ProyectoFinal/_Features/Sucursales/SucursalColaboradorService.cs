using AutoMapper;
using David.Academia.SistemaViajes.ProyectoFinal._Features._Common;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class SucursalColaboradorService
    {
        private readonly SistemaTransporteDrContext _context;
        private readonly IMapper _mapper;

        public SucursalColaboradorService(SistemaTransporteDrContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Respuesta<bool>> AgregarRelacionEntreSucursalColaborador(int colaboradorId, int sucursalId)
        {
            var respuesta = new Respuesta<bool>();
            if (colaboradorId <= 0 || sucursalId <= 0)
            {
                respuesta.Valido = false;
                respuesta.Mensaje = "Se debe de recibir un colaborador y colaborador valido.";
                return respuesta;
            }
            try
            {
                var existeColaborador = await _context.Colaboradores.AnyAsync(c => c.ColaboradorId == colaboradorId);
                var existeSucursal = await _context.Sucursales.AnyAsync(s => s.SucursalId == sucursalId);

                if (!existeColaborador || !existeSucursal)
                {
                    respuesta.Valido = false;
                    respuesta.Mensaje = "El colaborador o la sucursal no existe en la base de datos.";
                    return respuesta;
                }

                var sucursalColaborador = new SucursalColaborador()
                {
                    ColaboradorId = colaboradorId,
                    SucursalId = sucursalId
                };

                await _context.AddAsync(sucursalColaborador);
                await _context.SaveChangesAsync();

                respuesta.Mensaje = "Relacion entre colaborador y sucursal guardada con éxito.";

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
