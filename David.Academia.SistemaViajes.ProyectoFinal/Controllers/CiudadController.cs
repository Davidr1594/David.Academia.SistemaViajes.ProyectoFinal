using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadController : ControllerBase
    {
        private readonly CiudadService _ciudadService;

        public CiudadController(CiudadService ciudadService)
        { 
            _ciudadService = ciudadService;
        }

        [HttpPost("CrearCiudad")]
        public async Task<IActionResult> CreaCiudad([FromBody] CiudadDto ciudad)
        {
            var respuesta = await _ciudadService.CrearCiudad(ciudad);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerCiudades")]
        public async Task<IActionResult> ObtenerCiudades()
        {
            var respuesta = await _ciudadService.ObtenerCiudades();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerCiudadPorId/{ciudadId}")]
        public async Task<IActionResult> ObtenerCiudadPorId(int ciudadId)
        {
            var respuesta = await _ciudadService.ObtenerCiudadPorId(ciudadId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarCiudad")]
        public async Task<IActionResult> ActualizarCiudad([FromBody] CiudadDto ciudad)
        {
            var respuesta = await _ciudadService.ActualizarCiudad(ciudad);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoCiudad/{ciudadId}/estado")]
        public async Task<IActionResult> EstadoCiudad(int ciudadId, [FromQuery] bool estado)
        {
            var respuesta = await _ciudadService.EstadoCiudad(ciudadId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

    }
}
