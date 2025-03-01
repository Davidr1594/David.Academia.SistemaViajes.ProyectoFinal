
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.EstadoDePago.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoPagoController : ControllerBase
    {
        private readonly EstadoPagoService _estadoPagoService;

        public EstadoPagoController (EstadoPagoService estadoPagoService)
        {
            _estadoPagoService = estadoPagoService;
        }

        [HttpPost("CrearEstadoPago")]
        public async Task<IActionResult> CreaEstadoPago([FromBody] EstadoPagoDto estadoPago)
        {
            var respuesta = await _estadoPagoService.CrearEstadoPago(estadoPago);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerEstadosPagos")]
        public async Task<IActionResult> ObtenerEstadosPagos()
        {
            var respuesta = await _estadoPagoService.ObtenerEstadosPagos();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerEstadoPagoPorId/{estadoPagoId}")]
        public async Task<IActionResult> ObtenerEstadoPagoPorId(int estadoPagoId)
        {
            var respuesta = await _estadoPagoService.ObtenerEstadoPagoPorId(estadoPagoId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarEstadoPago")]
        public async Task<IActionResult> ActualizarEstadoPago([FromBody] EstadoPagoDto estadoPago)
        {
            var respuesta = await _estadoPagoService.ActualizarRol(estadoPago);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoDeEstadoPago/{estadoPagoId}/estado")]
        public async Task<IActionResult> EstadoDeEstadoPago(int estadoPagoId, [FromQuery] bool estado)
        {
            var respuesta = await _estadoPagoService.EstadoDeEstadoPago(estadoPagoId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
