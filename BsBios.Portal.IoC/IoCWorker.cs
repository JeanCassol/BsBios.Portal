using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using StructureMap;

namespace BsBios.Portal.IoC
{
    public static class IoCWorker
    {
        public static void Configure()
        {
            ObjectFactory.Configure(x =>
            {
                //Se o projeto utilizar o padrão repositório ativar o unit of work
                x.For<IUnitOfWork>()
                              .HybridHttpOrThreadLocalScoped()
                              .Use<UnitOfWorkNh>();
                x.For<IUnitOfWorkNh>()
                              .HybridHttpOrThreadLocalScoped()
                              .Use<UnitOfWorkNh>();
                x.AddRegistry<AplicationServiceRegistry>();
                x.AddRegistry<DomainServiceRegistry>();
                x.AddRegistry<RepositoryRegistry>();
                x.AddRegistry<InfraServiceRegistry>();
            });
        }
    }
}
