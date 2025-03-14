using David.Academia.SistemaViajes.ProyectoFinal.Infrastructure.SistemaTransporteDrDataBase.Entities;
using Farsiman.Domain.Core.Standard.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace David.Academia.SistemaViajes.IntregrationTests.Mocks.Infranstructure
{
    public static class UnitOfWorkMock
    {
        public static IUnitOfWork GetMock()
        {
            // Creación del mock principal de IUnitOfWork
            var unitOfWork = Substitute.For<IUnitOfWork>();

            // Métodos estándar que retornan Task (async):
            unitOfWork.SaveChangesAsync().Returns(Task.FromResult(true));
            unitOfWork.BeginTransactionAsync().Returns(Task.CompletedTask);
            unitOfWork.CommitAsync().Returns(Task.CompletedTask);
            unitOfWork.RollBackAsync().Returns(Task.CompletedTask);

            // 🔹 Se crean repositorios mock para entidades específicas
 
            var colaboradorRepo = Substitute.For<IRepository<Colaborador>>();


            // Registra cada repositorio dentro del UnitOfWork mock
 
            unitOfWork.Repository<Colaborador>().Returns(colaboradorRepo);


            return unitOfWork;
        }
    }
}
