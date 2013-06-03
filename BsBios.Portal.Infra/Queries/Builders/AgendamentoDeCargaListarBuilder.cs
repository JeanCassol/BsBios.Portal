using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class AgendamentoDeCargaListarBuilder : BuilderMulti<AgendamentoDeCarga, Usuario, AgendamentoDeCargaListarVm>
    {
        private readonly IVerificaPermissaoAgendamento _verificaPermissaoAgendamento;

        public AgendamentoDeCargaListarBuilder(IVerificaPermissaoAgendamento verificaSePodeEditarAgendamento)
        {
            _verificaPermissaoAgendamento = verificaSePodeEditarAgendamento;
        }

        public override AgendamentoDeCargaListarVm BuildSingle(AgendamentoDeCarga model, Usuario usuario)
        {
            return new AgendamentoDeCargaListarVm
                {
                    IdQuota = model.Quota.Id ,
                    IdAgendamento = model.Id,
                    Peso = model.PesoTotal ,
                    Placa =  model.Placa,
                    Realizado = model.Realizado ? "Sim": "N�o",
                    PermiteEditar = _verificaPermissaoAgendamento.PermiteEditar(model, usuario)
                };
        }
    }
}
