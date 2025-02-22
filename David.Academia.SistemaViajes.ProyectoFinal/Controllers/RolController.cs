using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
  

    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly RolService _rolService;

        public RolController(RolService rolService)
        { 
            _rolService = rolService;
        }

        [HttpPost("CrearRol")]
        public async Task<IActionResult> CreaRol([FromBody] RolDto rol)
        {
            var respuesta = await _rolService.CrearRol(rol);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerRoles")]
        public async Task<IActionResult> ObtenerRoles()
        {
            var respuesta = await _rolService.ObtenerRoles();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerRolPorId/{rolId}")]
        public async Task<IActionResult> ObtenerRolPorId(int rolId)
        {
            var respuesta = await _rolService.ObtenerRolPorId(rolId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarRol")]
        public async Task<IActionResult> ActualizarRol([FromBody] RolDto rol)
        {
            var respuesta = await _rolService.ActualizarRol(rol);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }


        [HttpPatch("CambiarEstadoRol/{rolId}/estado")]
        public async Task<IActionResult> EstadoRol(int rolId, [FromQuery] bool estado)
        {
            var respuesta = await _rolService.EstadoRol(rolId,estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

    }
}
