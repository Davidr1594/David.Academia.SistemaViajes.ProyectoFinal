using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;

namespace David.Academia.SistemaViajes.ProyectoFinal._Features.Sucursales
{
    public class CiudadService
    {
        private readonly SistemaTransporteDrContext _context;

        public CiudadService(SistemaTransporteDrContext context)
        { 
            _context = context;
        }


    }
}
