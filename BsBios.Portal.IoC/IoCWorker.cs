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
                //x.For<IUnitOfWork>()
                //              .HybridHttpOrThreadLocalScoped()
                //              .Use<UnitOfWorkNh>();

                ////Forward indica para a interface IUnitOfWorkNh usará a mesma resolução que para a IUnitOfWork
                ////que é mais genérica.
                ////Com isso, nos serviços de aplicação eu posso utilizar a IUnitOfWork e se eu precisar
                ////outra implementação concreta,basta eu trocar aqui.
                ////Os serviços poderiam, por exemplo, começar a utilizar uma implementação de uma única UnitOfWork 
                ////com Entity Framework ou ADO e os serviços não precisariam ser alterados. 
                //x.Forward<IUnitOfWork, IUnitOfWorkNh>();

                x.For<UnitOfWorkNh>()
                   .HybridHttpOrThreadLocalScoped()
                   .Use<UnitOfWorkNh>();

                x.For<IUnitOfWorkNh>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use(ctx => ctx.GetInstance<UnitOfWorkNh>());

                x.For<IUnitOfWork>()
                 .HybridHttpOrThreadLocalScoped()
                 .Use(ctx => ctx.GetInstance<UnitOfWorkNh>());

                x.AddRegistry<AplicationServiceRegistry>();
                x.AddRegistry<DomainServiceRegistry>();
                x.AddRegistry<RepositoryRegistry>();
                x.AddRegistry<InfraServiceRegistry>();
            });
        }
    }
}
