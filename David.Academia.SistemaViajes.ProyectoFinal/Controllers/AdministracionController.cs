
using David.Academia.SistemaViajes.ProyectoFinal._Features.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace David.Academia.SistemaViajes.ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministracionController : ControllerBase
    {
        private readonly RolService _rolService;

        public AdministracionController(RolService rolService)
        {
            _rolService = rolService;
        }


        //[HttpGet("ObtenerRoles")]
        //public IActionResult ObtenerRoles()
        //{
        //    //return Ok(_rolService.ObtenerRoles());
        //}
    }
}
