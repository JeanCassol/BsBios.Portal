using StructureMap;

namespace BsBios.Portal.Infra.IoC
{
    public static class IoCWorker
    {
        public static void Configure()
        {
            ObjectFactory.Configure(x =>
            {
                //Se o projeto utilizar o padrão repositório ativar o unit of work
                //x.For<IUnitOfWork>()
                //              .HybridHttpOrThreadLocalScoped()
                //              .Use<UnitOfWork.UnitOfWork>();
                x.AddRegistry<AplicationServiceRegistry>();
                x.AddRegistry<RepositoryRegistry>();
                x.AddRegistry<InfraServiceRegistry>();
            });
        }
    }
}
