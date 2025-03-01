using David.Academia.SistemaViajes.ProyectoFinal._Features.Reportes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly ReportesService _reportesService;

        public ReporteController(ReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpGet("ObtenerReporteViajes/")]
        public async Task<IActionResult> ObtenerReporteViajes([FromQuery]int transportistaId,[FromQuery]DateTime fechaInicio, [FromQuery]DateTime fechaFin)
        { 
            var respuesta = await _reportesService.TraerReporteDeViajesPorFechas(transportistaId, fechaInicio, fechaFin);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpGet("ObtenerReporteViajesDetalle/")]
        public async Task<IActionResult> ObtenerReporteViajesDetalle([FromQuery]int viajeId)
        {
            var respuesta = await _reportesService.traerReportePorColaborador(viajeId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
