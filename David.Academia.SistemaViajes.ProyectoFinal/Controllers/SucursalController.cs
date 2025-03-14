using David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales;
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



        public SucursalController(SucursalService sucursalService)
        { 
            _sucursalService = sucursalService;

        }

        [HttpPost("CrearSucursal")]
        public async Task<IActionResult> CrearSucursal([FromQuery] int UsuarioCread,[FromBody] SucursalDto sucursalDto)
        {
            var respuesta = await _sucursalService.CrearSucursal(UsuarioCread,sucursalDto);

            if (!respuesta.Valido)
            {
                return Ok(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerSucursal")]
        public async Task<IActionResult> ObtenerSucursal()
        {
            var respuesta = await _sucursalService.ObtenerSucursales();

            if (!respuesta.Valido)
            {
                return Ok(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
        [HttpGet("ObtenerSucursalesColaboradores")]
        public async Task<IActionResult> ObtenerSucursalesColaboradores()
        {
            var respuesta = await _sucursalService.ObtenerSucursalColaboradores();

            if (!respuesta.Valido)
            {
                return Ok(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpGet("ObtenerSucursalPorId")]
        public async Task<IActionResult> ObtenerSucursalPorId([FromQuery]int sucursalId)
        {
            var respuesta = await _sucursalService.ObtenerSucursalPorId(sucursalId);

            if (!respuesta.Valido)
            {
                return BadRequest(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }

        [HttpPut("ActualizarSucursal")]
        public async Task<IActionResult> ActualizarSucursal([FromQuery]int usuarioActualizaId, [FromBody] SucursalDto sucursalDto)
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

        [HttpPost("AgregarRelacionSucursalColaborador")]
        public async Task<IActionResult> AgregarRelacionSucursalColaborador([FromQuery]int sucursalId,[FromQuery]int colaboradorId) 
        {
            var respuesta = await _sucursalService.AgregarRelacionEntreSucursalColaborador(sucursalId, colaboradorId);

            if (!respuesta.Valido)
            {
                return Ok(new { respuesta.Mensaje, respuesta.DetalleError });
            }
            return Ok(respuesta);
        }
    }
}
