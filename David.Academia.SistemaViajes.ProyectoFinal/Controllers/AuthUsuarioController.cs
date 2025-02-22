using David.Academia.SistemaViajes.ProyectoFinal._Features.Seguridad;
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

        [HttpGet("AutenticarUsuario")]
        public async Task<IActionResult> AuntenticarUsuario(string clave, string usuario)
        {
            var respuesta = await _authService.AutenticarUsuario(clave, usuario);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }

            return Ok(respuesta);

        }
    }
}
