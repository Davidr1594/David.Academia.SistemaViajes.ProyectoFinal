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
                return Ok(new { respuesta.Mensaje, respuesta.DetalleError });
            }

            return Ok(respuesta);

        }
    }
}
