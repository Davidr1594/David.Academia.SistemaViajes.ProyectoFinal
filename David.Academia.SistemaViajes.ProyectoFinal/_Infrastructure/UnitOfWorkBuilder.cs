using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase;
using Farsiman.Domain.Core.Standard.Repositories;
using Farsiman.Infraestructure.Core.Entity.Standard;

namespace David.Academia.SistemaViajes.ProyectoFinal._Infrastructure
{
    public class UnitOfWorkBuilder
    {

        IServiceProvider _serviceProvider;

        public UnitOfWorkBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork BuildSistemaDeTransporte()
        {
            var dbContext = _serviceProvider.GetRequiredService<SistemaTransporteDrContext>();
            return new UnitOfWork(dbContext);
        }

    }
}
