using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Transportistas.Dto;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportistaController : ControllerBase
    {
        private readonly TransportistaService _transportistaService;

        public TransportistaController(TransportistaService transportistaService)
        {
            _transportistaService = transportistaService;
        }

        [HttpPost("CrearTransportista/{usuarioCreaId}")]
        public async Task<IActionResult> CrearUsuario(int usuarioCreaId, [FromBody] TransportistaDto transportista)
        {
            var respuesta = await _transportistaService.CrearTransportista(usuarioCreaId, transportista);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpGet("ObtenerTransportistas")]
        public async Task<IActionResult> ObtenerTransportistas()
        {
            var respuesta = await _transportistaService.ObtenerTransportistas();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpGet("ObtenerTransportistaPorId/{transportistaId}")]
        public async Task<IActionResult> ObtenerTransportistaPorId(int transportistaId)
        {
            var respuesta = await _transportistaService.ObtenerTransportistaPorId(transportistaId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPut("ActualizarTransportita/{usuarioActualizaId}")]
        public async Task<IActionResult> ActualizarTransportista(int usuarioActualizaId, [FromBody] TransportistaDto transportista)
        {
            var respuesta = await _transportistaService.ActualizarTransportista(usuarioActualizaId, transportista);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoTransportista/{transportista}/estado")]
        public async Task<IActionResult> EstadoRol(int transportistaId, [FromQuery] bool estado)
        {
            var respuesta = await _transportistaService.EstadoTransportista(transportistaId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
