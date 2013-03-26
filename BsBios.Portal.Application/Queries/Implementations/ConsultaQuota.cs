using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaQuota : IConsultaQuota
    {
        private readonly IQuotas _quotas;
        private readonly IBuilder<Quota, QuotaConsultarVm> _builderQuota;
        private readonly IBuilder<Quota, QuotaPorFornecedorVm> _builderQuotaPorFornecedor;
        private readonly IBuilder<AgendamentoDeCarga, AgendamentoDeCargaListarVm> _builderAgendamentoDeCargaListar;
        private readonly IBuilder<AgendamentoDeCarga, AgendamentoDeCargaCadastroVm> _builderAgendamentoDeCargaCadastro;

        public ConsultaQuota(IQuotas quotas, IBuilder<Quota, QuotaConsultarVm> builderQuota, 
            IBuilder<Quota, QuotaPorFornecedorVm> builderQuotaPorFornecedor, IBuilder<AgendamentoDeCarga, AgendamentoDeCargaListarVm> builderAgendamentoDeCargaListar, 
            IBuilder<AgendamentoDeCarga, AgendamentoDeCargaCadastroVm> builderAgendamentoDeCargaCadastro)
        {
            _quotas = quotas;
            _builderQuota = builderQuota;
            _builderQuotaPorFornecedor = builderQuotaPorFornecedor;
            _builderAgendamentoDeCargaListar = builderAgendamentoDeCargaListar;
            _builderAgendamentoDeCargaCadastro = builderAgendamentoDeCargaCadastro;
        }

        public bool PossuiQuotaNaData(DateTime data)
        {
            return _quotas.FiltraPorData(data).Count() > 0;
        }

        public IList<QuotaConsultarVm> QuotasDaData(DateTime data)
        {
            return _builderQuota.BuildList(_quotas.FiltraPorData(data).List());
        }

        public KendoGridVm ListarQuotasDoFornecedor(PaginacaoVm paginacaoVm, string codigoDoFornecedor)
        {
            _quotas.DoFornecedor(codigoDoFornecedor);
            return new KendoGridVm
                {
                    QuantidadeDeRegistros = _quotas.Count() ,
                    Registros = _builderQuotaPorFornecedor.BuildList(_quotas
                    .GetQuery()
                    .OrderByDescending(x => x.Data)
                    .Skip(paginacaoVm.Skip)
                    .Take(paginacaoVm.Take).ToList())
                            .Cast<ListagemVm>()
                            .ToList()
                             
                };
        }

        public QuotaPorFornecedorVm ConsultarQuota(int idQuota)
        {
            return _builderQuotaPorFornecedor.BuildSingle(_quotas.BuscaPorId(idQuota));
        }

        public KendoGridVm ListarAgendamentosDaQuota(int idQuota)
        {
            Quota quota = _quotas.BuscaPorId(idQuota);
            return new KendoGridVm
                {
                    QuantidadeDeRegistros = quota.Agendamentos.Count,
                    Registros = _builderAgendamentoDeCargaListar.BuildList(quota.Agendamentos ).Cast<ListagemVm>().ToList()
                };
        }

        public AgendamentoDeCargaCadastroVm ConsultarAgendamento(int idQuota, int idAgendamento)
        {
            Quota quota = _quotas.BuscaPorId(idQuota);
            AgendamentoDeCarga agendamentoDeCarga = quota.Agendamentos.Single(x => x.Id == idAgendamento);
            return _builderAgendamentoDeCargaCadastro.BuildSingle(agendamentoDeCarga);
        }
    }
}