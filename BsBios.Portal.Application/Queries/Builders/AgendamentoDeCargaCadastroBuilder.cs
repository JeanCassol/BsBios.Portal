using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;
using StructureMap.Query;

namespace BsBios.Portal.Application.Queries.Builders
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
            AgendamentoDeCargaCadastroVm viewModel = null;
            if (model is AgendamentoDeCarregamento)
            {
                var modelConvertido = (AgendamentoDeCarregamento) model;
                viewModel = new AgendamentoDeCarregamentoCadastroVm
                    {
                        Motorista = modelConvertido.Motorista ,
                        Destino = modelConvertido.Destino,
                        Peso = modelConvertido.PesoTotal ,
                        Cabecalho = new AgendamentoDeCarregamentoCabecalhoVm
                        {
                            FluxoDeCarga =modelConvertido.Quota.FluxoDeCarga.Descricao() ,
                            Material = modelConvertido.Quota.Material.Descricao(),
                            Transportadora = modelConvertido.Quota.Fornecedor.Nome
                        },
                        ViewDeCadastro = "AgendamentoDeCarregamento",

                    };       
            }

            if (model is AgendamentoDeDescarregamento)
            {
                viewModel = new AgendamentoDeDescarregamentoCadastroVm
                    {
                        ViewDeCadastro = "AgendamentoDeDescarregamento",
                    };
            }

            if (viewModel != null)
            {
                viewModel.IdQuota = model.Quota.Id;
                viewModel.IdAgendamento = model.Id;
                viewModel.Placa = model.Placa;
                viewModel.DescricaoDoTerminal = model.Quota.Terminal.Nome;
                viewModel.PermiteEditar = permiteEditar;
                viewModel.PermiteRealizar = permiteRealizar;

            }

            return viewModel;
        }
    }
}
