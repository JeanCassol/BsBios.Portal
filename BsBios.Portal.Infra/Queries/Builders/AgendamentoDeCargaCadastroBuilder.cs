using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{
    public class AgendamentoDeCargaCadastroBuilder: BuilderMulti<AgendamentoDeCarga, Usuario,AgendamentoDeCargaCadastroVm>
    {
        private readonly IVerificaPermissaoAgendamento _verificaPermissaoAgendamento;

        public AgendamentoDeCargaCadastroBuilder(IVerificaPermissaoAgendamento verificaSePodeEditarAgendamento)
        {
            _verificaPermissaoAgendamento = verificaSePodeEditarAgendamento;
        }

        public override AgendamentoDeCargaCadastroVm BuildSingle(AgendamentoDeCarga model, Usuario usuario)
        {
            bool permiteEditar = _verificaPermissaoAgendamento.PermiteEditar(model, usuario);
            bool permiteRealizar = _verificaPermissaoAgendamento.PermiteRealizar(model, usuario);
            if (model is AgendamentoDeCarregamento)
            {
                return new AgendamentoDeCarregamentoCadastroVm
                    {
                        IdQuota = model.Quota.Id,
                        IdAgendamento = model.Id,
                        Placa = model.Placa ,
                        Peso = model.PesoTotal ,
                        ViewDeCadastro = "AgendamentoDeCarregamento",
                        PermiteEditar =  permiteEditar,
                        PermiteRealizar = permiteRealizar
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
                        PermiteEditar = permiteEditar,
                        PermiteRealizar = permiteRealizar
                    };
            }

            return null;
        }
    }
}
