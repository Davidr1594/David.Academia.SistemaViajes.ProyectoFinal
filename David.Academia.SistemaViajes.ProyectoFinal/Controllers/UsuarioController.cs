using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> CrearUsuario([FromQuery]int usuarioCreaId,[FromBody]UsuarioDto usuario)
        {
            var respuesta = await _usuarioService.CrearUsuario(usuarioCreaId,usuario);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var respuesta = await _usuarioService.ObtenerUsuarios();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpGet("ObtenerUsuarioPorId")]
        public async Task<IActionResult> ObtenerUsuarioPorId([FromQuery]int usuarioId)
        {
            var respuesta = await _usuarioService.ObtenerUsuarioPorId(usuarioId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPut("ActualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuario([FromQuery]int usuarioActualizaId, [FromBody] UsuarioDto usuario)
        {
            var respuesta= await _usuarioService.ActualizarUsuario(usuarioActualizaId, usuario);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoUsuario/{usuarioId}")]
        public async Task<IActionResult> EstadoRol(int usuarioId, [FromQuery] bool estado)
        {
            var respuesta = await _usuarioService.EstadoUsuario(usuarioId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
