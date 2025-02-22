using AutoMapper;
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
    public class PaisController : ControllerBase
    {
        private readonly PaisService _paisService;
        private readonly IMapper _mapper;

        public PaisController(PaisService paisService, IMapper mapper)
        { 
            _paisService = paisService;
            _mapper = mapper;
        }

        [HttpPost("CrearPais")]
        public async Task<IActionResult> CreaPais([FromBody] PaisDto paisDto)
        {
            var respuesta = await _paisService.CrearPais(paisDto);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerPaises")]
        public async Task<IActionResult> ObtenerPaises()
        {
            var respuesta = await _paisService.ObtenerPaises();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerPaisPorId/{paisId}")]
        public async Task<IActionResult> ObtenerPaisPorId(int paisId)
        {
            var respuesta = await _paisService.ObtenerPaisPorId(paisId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarPais/{paisId}")]
        public async Task<IActionResult> ActualizarPais(int paisId, [FromBody] PaisDto pais)
        {
            var respuesta = await _paisService.ActualizarPais(paisId, pais);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("CambiarEstadoPais/{paisId}/estado")]
        public async Task<IActionResult> EstadoPais(int paisId, [FromQuery] bool estado)
        {
            var respuesta = await _paisService.EstadoPais(paisId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
