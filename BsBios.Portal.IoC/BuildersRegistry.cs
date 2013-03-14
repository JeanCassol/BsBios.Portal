using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class BuildersRegistry : Registry
    {
        public BuildersRegistry()
        {
            For<IBuilder<Fornecedor, FornecedorCadastroVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<FornecedorCadastroBuilder>();
            For<IBuilder<Produto, ProdutoCadastroVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProdutoCadastroBuilder>();
            For<IBuilder<CondicaoDePagamento, CondicaoDePagamentoCadastroVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CondicaoPagamentoCadastroBuilder>();
            For<IBuilder<Incoterm, IncotermCadastroVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<IncotermCadastroBuilder>();
            For<IBuilder<Iva, IvaCadastroVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<IvaCadastroBuilder>();
            For<IBuilder<Usuario, UsuarioConsultaVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<UsuarioConsultaBuilder>();
            For<IBuilder<Enumeradores.Perfil, PerfilVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<PerfilBuilder>();
            For<IBuilder<UnidadeDeMedida, UnidadeDeMedidaCadastroVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<UnidadeDeMedidaCadastroBuilder>();
        }
    }
}