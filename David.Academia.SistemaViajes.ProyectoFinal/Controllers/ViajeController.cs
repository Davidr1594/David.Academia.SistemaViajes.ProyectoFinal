using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViajeController : ControllerBase
    {
        private readonly ViajeService _viajeService;

        public ViajeController(ViajeService viajeService)
        {
            _viajeService = viajeService;
        }

        [HttpPost("CrearViaje/{usuarioCreaId}")]
        public async Task<IActionResult> CrearViaje(int usuarioCreaId, [FromBody] ViajeDto viaje)
        {
            var respuesta = await _viajeService.CrearViaje(usuarioCreaId, viaje);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerViajes")]
        public async Task<IActionResult> ObtenerViajes()
        {
            var respuesta = await _viajeService.ObtenerViajes();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerViajePorId/{viajeId}")]
        public async Task<IActionResult> ObtenerViajePorId(int viajeId)
        {
            var respuesta = await _viajeService.ObtenerViajePorId(viajeId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarViaje/{usuarioActualizaId}")]
        public async Task<IActionResult> ActualizarViaje(int usuarioActualizaId, [FromBody] ViajeDto viaje)
        {
            var respuesta = await _viajeService.ActualizarViaje(usuarioActualizaId, viaje);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("CambiarEstadoViaje/{viajeId}/estado")]
        public async Task<IActionResult> EstadoViaje(int viajeId, [FromQuery] bool estado)
        {
            var respuesta = await _viajeService.EstadoViaje(viajeId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
