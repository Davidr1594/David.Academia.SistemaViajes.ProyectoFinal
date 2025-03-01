
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Pagos.MetodoDePago.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetodoPagoController : ControllerBase
    {
        private readonly MetodoPagoService _metodoPagoService;

        public MetodoPagoController (MetodoPagoService metodoPagoService)
        {
            _metodoPagoService = metodoPagoService;
        }

        [HttpPost("CrearMetodoPago")]
        public async Task<IActionResult> CreaMetodoPago([FromBody] MetodoPagoDto metodoPago)
        {
            var respuesta = await _metodoPagoService.CrearMetodoPago(metodoPago);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerMetodosPagos")]
        public async Task<IActionResult> ObtenerMetodosPagos()
        {
            var respuesta = await _metodoPagoService.ObtenerMetodosPagos();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerMetodoPagoPorId/{metodoPagoId}")]
        public async Task<IActionResult> ObtenerMetodoPagoPorId(int metodoPagoId)
        {
            var respuesta = await _metodoPagoService.ObtenerMetodoPagoPorId(metodoPagoId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarMetodoPago")]
        public async Task<IActionResult> ActualizarMetodoPago([FromBody] MetodoPagoDto metodoPago)
        {
            var respuesta = await _metodoPagoService.ActualizarMetodoPago(metodoPago);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoMetodoPago/{rolId}/estado")]
        public async Task<IActionResult> EstadoMetodoPago(int metodoPagoId, [FromQuery] bool estado)
        {
            var respuesta = await _metodoPagoService.EstadoMetodoPago(metodoPagoId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
