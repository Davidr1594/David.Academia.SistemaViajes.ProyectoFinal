using David.Academia.SistemaViajes.ProyectoFinal._Features._Common.GoogleMaps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManejoDistanciaController : ControllerBase
    {
        private readonly ManejoDistanciasService _manejoDistanciaService;

        public ManejoDistanciaController(ManejoDistanciasService manejoDistanciasService)
        {
            _manejoDistanciaService = manejoDistanciasService;
        }

        [HttpGet("ObtenerDireccion")]
        public async Task<IActionResult> ObtenerDireccion([FromQuery] decimal latitud, [FromQuery] decimal longitud)
        {
            var direccion = await _manejoDistanciaService.ObtenerDireccionDesdeCordenadas(latitud, longitud);
            return Ok(direccion);
        }

        [HttpGet("Obtenerkms")]
        public async Task<IActionResult> ObtenerKms2Sucurrsales([FromQuery] decimal latitudSucursal, [FromQuery] decimal longitudSucursal, [FromQuery] decimal latitudColaborador, [FromQuery] decimal longitudColaborador)
        {
            var direccion = await _manejoDistanciaService.ObtenerDistanciaEntreSucursalColaborador(latitudSucursal, longitudSucursal, latitudColaborador, longitudColaborador);
            return Ok(direccion);
        }
    }
}
