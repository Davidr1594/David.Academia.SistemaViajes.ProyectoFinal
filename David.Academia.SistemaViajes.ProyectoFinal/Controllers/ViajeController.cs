using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Viajes.Viajes.Dto;
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

        [HttpPost("CrearViaje")]
        public async Task<IActionResult> CrearViaje([FromQuery]int usuarioCreaId, [FromBody] ViajeDto viaje)
        {
            var respuesta = await _viajeService.CrearViaje(usuarioCreaId, viaje);

            if (!respuesta.Valido)
            {
                return Ok(new { respuesta.Valido, respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPost("AgregarColaboradoresAViaje")]
        public async Task<IActionResult> AgregarColaboradoresAViaje([FromQuery]int usuarioCreaId, [FromQuery]int sucursalId, [FromBody] List<int> colaboradoresId)
        {
            var respuesta = await _viajeService.AgregarColaboradoresAViaje(usuarioCreaId, sucursalId, colaboradoresId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Valido, respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoViaje")]
        public async Task<IActionResult> CambiarEstadoViaje([FromQuery] int usuarioActualizaId,[FromQuery] int viajeId, [FromQuery] int estadoId)
        {
            var respuesta = await _viajeService.ActualizadoEstadoViaje(usuarioActualizaId,viajeId,estadoId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Valido, respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpGet("ObtenerViajes")]
        public async Task<IActionResult> ObtenerViajes()
        {
            var respuesta = await _viajeService.ObtenerViajes();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Valido, respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerViajePorId")]
        public async Task<IActionResult> ObtenerViajePorId([FromQuery]int viajeId)
        {
            var respuesta = await _viajeService.ObtenerViajePorId(viajeId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Valido, respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarViaje/{usuarioActualizaId}")]
        public async Task<IActionResult> ActualizarViaje(int usuarioActualizaId, [FromBody] ViajeDto viaje)
        {
            var respuesta = await _viajeService.ActualizarViaje(usuarioActualizaId, viaje);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Valido, respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("CambiarEstadoActivoViaje/{viajeId}/estado")]
        public async Task<IActionResult> EstadoViaje(int viajeId, [FromQuery] bool estado)
        {
            var respuesta = await _viajeService.EstadoViajeActivo(viajeId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Valido, respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
