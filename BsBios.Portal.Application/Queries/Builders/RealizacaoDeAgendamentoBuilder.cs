using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class RealizacaoDeAgendamentoBuilder : Builder<Enumeradores.RealizacaoDeAgendamento, RealizacaoDeAgendamentoVm>
    {
        public override RealizacaoDeAgendamentoVm BuildSingle(Enumeradores.RealizacaoDeAgendamento model)
        {
            return new RealizacaoDeAgendamentoVm
                {
                    Codigo = (int) model,
                    Descricao = model.Descricao(),
                    Padrao = (model == Enumeradores.RealizacaoDeAgendamento.NaoRealizado)
                };
        }
    }
}
