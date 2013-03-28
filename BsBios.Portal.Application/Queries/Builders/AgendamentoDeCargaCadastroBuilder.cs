using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{
    public class AgendamentoDeCargaCadastroBuilder: Builder<AgendamentoDeCarga,AgendamentoDeCargaCadastroVm>
    {
        public override AgendamentoDeCargaCadastroVm BuildSingle(AgendamentoDeCarga model)
        {
            if (model is AgendamentoDeCarregamento)
            {
                return new AgendamentoDeCarregamentoCadastroVm
                    {
                        IdQuota = model.Quota.Id,
                        IdAgendamento = model.Id,
                        Placa = model.Placa ,
                        Peso = model.PesoTotal ,
                        ViewDeCadastro = "AgendamentoDeCarregamento",
                        PermiteRealizar = !model.Realizado
                        
                    };       
            }

            if (model is AgendamentoDeDescarregamento)
            {
                return new AgendamentoDeDescarregamentoCadastroVm
                    {
                        IdQuota = model.Quota.Id ,
                        IdAgendamento = model.Id ,
                        Placa = model.Placa,
                        ViewDeCadastro = "AgendamentoDeDescarregamento",
                        PermiteRealizar = !model.Realizado
                    };
            }

            return null;
        }
    }
}
