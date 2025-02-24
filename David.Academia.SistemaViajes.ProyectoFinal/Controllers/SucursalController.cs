using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios.Dto;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales.Dto;
using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SucursalController : ControllerBase
    {
        private readonly SucursalService _sucursalService;
        private readonly SucursalColaboradorService _sucursalColaboradorService;


        public SucursalController(SucursalService sucursalService, SucursalColaboradorService sucursalColaboradorService)
        { 
            _sucursalService = sucursalService;
            _sucursalColaboradorService = sucursalColaboradorService;
        }

        [HttpPost("CrearSucursal")]
        public async Task<IActionResult> CrearSucursal(int UsuarioActualizaId,[FromBody] SucursalDto sucursalDto)
        {
            var respuesta = await _sucursalService.CrearSucursal(UsuarioActualizaId,sucursalDto);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerSucursal")]
        public async Task<IActionResult> ObtenerSucursal()
        {
            var respuesta = await _sucursalService.ObtenerSucursales();

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerSucursalPorId/{sucursalId}")]
        public async Task<IActionResult> ObtenerSucursalPorId(int sucursalId)
        {
            var respuesta = await _sucursalService.ObtenerSucursalPorId(sucursalId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarSucursal/{usuarioActualizaId}")]
        public async Task<IActionResult> ActualizarSucursal(int usuarioActualizaId, [FromBody] SucursalDto sucursalDto)
        {
            var respuesta = await _sucursalService.ActualizarSucursal(usuarioActualizaId, sucursalDto);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("CambiarEstadoSucursal/{sucursalId}/estado")]
        public async Task<IActionResult> EstadoSucursalColaborador(int sucursalId, [FromQuery] bool estado)
        {
            var respuesta = await _sucursalService.EstadoSucursal(sucursalId, estado);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPatch("AgregarRelacionSucursalColaborador/{sucursalId}/{colaboradorId}")]
        public async Task<IActionResult> AgregarRelacionSucursalColaborador(int sucursalId,int colaboradorId) 
        {
            var respuesta = await _sucursalColaboradorService.AgregarRelacionEntreSucursalColaborador(sucursalId, colaboradorId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
