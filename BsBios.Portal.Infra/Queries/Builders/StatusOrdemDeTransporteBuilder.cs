using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class StatusOrdemDeTransporteBuilder: Builder<Enumeradores.StatusDaOrdemDeTransporte, ChaveValorVm>
    {
        public override ChaveValorVm BuildSingle(Enumeradores.StatusDaOrdemDeTransporte model)
        {
            return new ChaveValorVm()
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao()
                };
        }
    }
}
