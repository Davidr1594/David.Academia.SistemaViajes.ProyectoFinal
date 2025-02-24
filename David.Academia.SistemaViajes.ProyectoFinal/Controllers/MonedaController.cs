using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonedaController : ControllerBase
    {
        private readonly MonedaService _monedaService;

        public MonedaController (MonedaService monedaService)
        {
            _monedaService = monedaService;
        }

        [HttpPost("CrearMoneda")]
        public async Task<IActionResult> CreaMoneda([FromBody] MonedaDto moneda)
        {
            var respuesta = await _monedaService.CrearMoneda(moneda);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerMonedas")]
        public async Task<IActionResult> ObtenerMonedas()
        {
            var respuesta = await _monedaService.ObtenerMonedas();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerMonedaPorId/{monedaId}")]
        public async Task<IActionResult> ObtenerRolPorId(int monedaId)
        {
            var respuesta = await _monedaService.ObtenerMonedaPorId(monedaId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarMoneda")]
        public async Task<IActionResult> ActualizarMoneda([FromBody] MonedaDto moneda)
        {
            var respuesta = await _monedaService.ActualizarMoneda(moneda);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoMoneda/{monedaId}/estado")]
        public async Task<IActionResult> EstadoRol(int monedaId, [FromQuery] bool estado)
        {
            var respuesta = await _monedaService.EstadoMoneda(monedaId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
