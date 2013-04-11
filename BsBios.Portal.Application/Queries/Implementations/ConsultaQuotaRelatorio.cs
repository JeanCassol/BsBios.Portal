using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaQuotaRelatorio : IConsultaQuotaRelatorio
    {
        private readonly IQuotas _quotas;

        public ConsultaQuotaRelatorio(IQuotas quotas)
        {
            _quotas = quotas;
        }

        public IList<QuotaPlanejadoRealizadoVm> PlanejadoRealizado(RelatorioAgendamentoFiltroVm filtro)
        {
            if (filtro.DataDe.HasValue)
            {
                _quotas.APartirDe(filtro.DataDe.Value);
            }
            if (filtro.DataAte.HasValue)
            {
                _quotas.Ate(filtro.DataAte.Value);
            }
            if (filtro.CodigoFluxoDeCarga.HasValue)
            {
                _quotas.FiltraPorFluxo(
                    (Enumeradores.FluxoDeCarga)
                    Enum.Parse(typeof (Enumeradores.FluxoDeCarga), Convert.ToString(filtro.CodigoFluxoDeCarga.Value)));
            }
            if (!string.IsNullOrEmpty(filtro.CodigoFornecedor))
            {
                _quotas.DoFornecedor(filtro.CodigoFornecedor);
            }

            _quotas.DoTerminal(filtro.CodigoTerminal);


            var quotas = (from quota in _quotas.GetQuery()
                          group quota by new {quota.CodigoTerminal, quota.Fornecedor.Nome, quota.FluxoDeCarga}
                          into agrupador
                          select new
                              {
                                  agrupador.Key,
                                  PlanejadoTotal = agrupador.Sum(x => x.PesoTotal),
                                  RealizadoTotal = agrupador.Sum(x => x.PesoRealizado)
                              }).ToList();


            return quotas.Select(x =>
                                 new QuotaPlanejadoRealizadoVm
                                     {
                                         CodigoTerminal = x.Key.CodigoTerminal,
                                         NomeDoFornecedor = x.Key.Nome,
                                         FluxoDeCarga = x.Key.FluxoDeCarga.Descricao(),
                                         Quota = x.PlanejadoTotal,
                                         PesoRealizado = x.RealizadoTotal
                                     }).ToList();

        }
    }
}