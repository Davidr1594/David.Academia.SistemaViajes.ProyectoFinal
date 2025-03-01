using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoDepartamentoController : ControllerBase
    {
        private readonly EstadoDepartamentoService _estadoDepartamentoService;

        public EstadoDepartamentoController (EstadoDepartamentoService estadoDepartamentoService)
        {
            _estadoDepartamentoService = estadoDepartamentoService;
        }

        [HttpPost("CrearEstadoDepartamento")]
        public async Task<IActionResult> CreaRol([FromBody] EstadoDepartamentoDto estadoDepartamento)
        {
            var respuesta = await _estadoDepartamentoService.CrearEstadoDepartamento(estadoDepartamento);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerEstadosDepartamentos")]
        public async Task<IActionResult> ObtenerEstadosDepartamentos()
        {
            var respuesta = await _estadoDepartamentoService.ObtenerEstadosDepartamentos();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerEstadoDepartamentoPorId/{estadoDepartamentoId}")]
        public async Task<IActionResult> ObtenerEstadoDepartamentoPorId(int estadoDepartamentoId)
        {
            var respuesta = await _estadoDepartamentoService.ObtenerEstadoDepartamentoPorId(estadoDepartamentoId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarEstadoDepartamento")]
        public async Task<IActionResult> ActualizarEstadoDepartamento([FromBody] EstadoDepartamentoDto estadoDepartamento)
        {
            var respuesta = await _estadoDepartamentoService.ActualizarEstadoDepartamento(estadoDepartamento);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoDeEstadoDepartamento/{estadoDepartamentoId}/estado")]
        public async Task<IActionResult> EstadoRol(int estadoDepartamentoId, [FromQuery] bool estado)
        {
            var respuesta = await _estadoDepartamentoService.EstadoDeEstadoDepartamento(estadoDepartamentoId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

    }
}
