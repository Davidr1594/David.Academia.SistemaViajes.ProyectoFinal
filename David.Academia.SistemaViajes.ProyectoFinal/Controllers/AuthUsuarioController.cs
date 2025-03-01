using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUsuarioController : ControllerBase
    {

        private readonly AuthService _authService;

        public AuthUsuarioController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("AutenticarUsuario")]
        public async Task<IActionResult> AuntenticarUsuario([FromBody]UsuarioAuthDto usuario)
        {
            var respuesta = await _authService.AutenticarUsuario(usuario);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            if (!respuesta.Datos)
            {
                return Unauthorized(new { mensaje = respuesta.Mensaje });
            }


            return Ok(respuesta);

        }
    }
}
