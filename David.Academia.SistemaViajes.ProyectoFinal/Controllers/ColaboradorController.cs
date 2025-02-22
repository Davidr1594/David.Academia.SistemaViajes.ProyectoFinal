using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboradorController : ControllerBase
    {
        private readonly ColaboradorService _colaboradorService;

        public ColaboradorController(ColaboradorService colaboradorService)
        {
            _colaboradorService = colaboradorService;
        }

        [HttpPost("CrearColaborador/{usuarioCreaId}")]
        public async Task<IActionResult> CrearColaborador(int usuarioCreaId, [FromBody] ColaboradorDto usuario)
        {
            var respuesta = await _colaboradorService.CrearColaborador(usuarioCreaId, usuario);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerColaboradores")]
        public async Task<IActionResult> ObtenerColaboradores()
        {
            var respuesta = await _colaboradorService.ObtenerColaboradores();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerColaboradorPorId/{colaboradorId}")]
        public async Task<IActionResult> ObtenerColaboradorPorId(int colaboradorId)
        {
            var respuesta = await _colaboradorService.ObtenerColaboradorPorId(colaboradorId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarColaborador/{usuarioActualizaId}")]
        public async Task<IActionResult> ActualizarColaborador(int usuarioActualizaId, [FromBody] ColaboradorDto colaborador)
        {
            var respuesta = await _colaboradorService.ActualizarColaborador(usuarioActualizaId, colaborador);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("CambiarEstadoColaborador/{colaboradorId}/estado")]
        public async Task<IActionResult> EstadoColaborador(int colaboradorId, [FromQuery] bool estado)
        {
            var respuesta = await _colaboradorService.EstadoColaborador(colaboradorId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
