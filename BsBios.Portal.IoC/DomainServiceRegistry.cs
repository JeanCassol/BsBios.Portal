using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.Services.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class DomainServiceRegistry:  Registry
    {
        public DomainServiceRegistry()
        {
            For<ISelecionaFornecedor>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<SelecionaFornecedor>();
            For<ICadastroProdutoOperacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroProdutoOperacao>();
            For<ICadastroFornecedorOperacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroFornecedorOperacao>();
            For<ICadastroCondicaoPagamentoOperacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroCondicaoPagamentoOperacao>();
            For<ICadastroIvaOperacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroIvaOperacao>();
            For<ICadastroUsuarioOperacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroUsuarioOperacao>();
        }
    }
}
