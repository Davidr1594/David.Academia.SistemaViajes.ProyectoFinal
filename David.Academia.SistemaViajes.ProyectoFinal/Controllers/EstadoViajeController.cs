using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoViajeController : ControllerBase
    {
        private readonly EstadoViajeService _estadoViajeService;

        public EstadoViajeController (EstadoViajeService estadoViajeService)
        {
            _estadoViajeService = estadoViajeService;
        }

        [HttpPost("CrearEstadoViaje")]
        public async Task<IActionResult> CrearEstadoViaje([FromBody] EstadoViajeDto estadoViaje)
        {
            var respuesta = await _estadoViajeService.CrearEstadoViaje(estadoViaje);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerEstadosViajes")]
        public async Task<IActionResult> ObtenerEstadosViajes()
        {
            var respuesta = await _estadoViajeService.ObtenerEstadosViajes();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerEstadoViajePorId/{estadoViajeId}")]
        public async Task<IActionResult> ObtenerRolPorId(int estadoViajeId)
        {
            var respuesta = await _estadoViajeService.ObtenerEstadoViajePorId(estadoViajeId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarEstadoViaje")]
        public async Task<IActionResult> ActualizarEstadoViaje([FromBody] EstadoViajeDto estadoViaje)
        {
            var respuesta = await _estadoViajeService.ActualizarEstadoViaje(estadoViaje);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoDeEstadoViaje/{estadoViajeId}/estado")]
        public async Task<IActionResult> EstadoDeEstadoViaje(int estadoViajeId, [FromQuery] bool estado)
        {
            var respuesta = await _estadoViajeService.EstadoDeEstadoViaje(estadoViajeId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
