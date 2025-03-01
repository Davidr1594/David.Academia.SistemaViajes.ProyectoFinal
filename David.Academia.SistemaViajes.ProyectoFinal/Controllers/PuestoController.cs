using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Puestos;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Colaboradores.Puestos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PuestoController : ControllerBase
    {
        private readonly PuestoService _puestoService;

        public PuestoController(PuestoService puestoService)
        { 
            _puestoService = puestoService;
        }

        [HttpPost("CrearPuesto")]
        public async Task<IActionResult> CrearPuesto([FromBody] PuestoDto puesto)
        {
            var respuesta = await _puestoService.CrearPuesto(puesto);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerPuestos")]
        public async Task<IActionResult> ObtenerPuestos()
        {
            var respuesta = await _puestoService.ObtenerPuestos();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerPuestoPorId/{puestoId}")]
        public async Task<IActionResult> ObtenerPuestoPorId(int puestoId)
        {
            var respuesta = await _puestoService.ObtenerPuestoPorId(puestoId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarPuesto")]
        public async Task<IActionResult> ActualizarPuesto([FromBody] PuestoDto puesto)
        {
            var respuesta = await _puestoService.ActualizarPuesto(puesto);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("CambiarEstadoPuesto/{puestoId}/estado")]
        public async Task<IActionResult> EstadoPuesto(int puestoId, [FromQuery] bool estado)
        {
            var respuesta = await _puestoService.EstadoPuesto(puestoId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

    }
}
