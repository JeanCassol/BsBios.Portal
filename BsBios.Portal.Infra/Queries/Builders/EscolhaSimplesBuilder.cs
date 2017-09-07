using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class EscolhaSimplesBuilder: Builder<Enumeradores.EscolhaSimples, ChaveValorVm>
    {
        public override ChaveValorVm BuildSingle(Enumeradores.EscolhaSimples model)
        {
            return new ChaveValorVm
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao()
                };
        }
    }
}
