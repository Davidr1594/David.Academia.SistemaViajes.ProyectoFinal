using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Generales.ParametrosSistema.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParametroSistemaController : ControllerBase
    {
        private readonly ParametroSistemaService _parametroSistemaService;

        public ParametroSistemaController (ParametroSistemaService parametroSistemaService)
        {
            _parametroSistemaService = parametroSistemaService;
        }

        [HttpPost("CrearParametro/{usuarioCreaId}")]
        public async Task<IActionResult> CrearParametro(int usuarioCreaId, [FromBody] ParametroSistemaDto parametro)
        {
            var respuesta = await _parametroSistemaService.CrearParametroSistema(usuarioCreaId, parametro);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerParametros")]
        public async Task<IActionResult> ObtenerParametros()
        {
            var respuesta = await _parametroSistemaService.ObtenerParametrosSistema();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerParametroPorId/{parametroId}")]
        public async Task<IActionResult> ObtenerParametroPorId(int parametroId)
        {
            var respuesta = await _parametroSistemaService.ObtenerParametroSistemaPorId(parametroId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarParametro/{usuarioActualizaId}")]
        public async Task<IActionResult> ActualizarParametro(int usuarioActualizaId, [FromBody] ParametroSistemaDto parametro)
        {
            var respuesta = await _parametroSistemaService.ActualizarParametroSistema(usuarioActualizaId, parametro);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("CambiarEstadoParametro/{parametroId}/estado")]
        public async Task<IActionResult> CambiarEstadoParametro(int parametroId, [FromQuery] bool estado)
        {
            var respuesta = await _parametroSistemaService.EstadoParametroSistema(parametroId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
