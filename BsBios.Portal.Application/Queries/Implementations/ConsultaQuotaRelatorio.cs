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

        private void AplicaFiltros(RelatorioAgendamentoFiltroVm filtro)
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
                    Enum.Parse(typeof(Enumeradores.FluxoDeCarga), Convert.ToString(filtro.CodigoFluxoDeCarga.Value)));
            }
            if (!string.IsNullOrEmpty(filtro.CodigoFornecedor))
            {
                _quotas.DoFornecedor(filtro.CodigoFornecedor);
            }

            _quotas.DoTerminal(filtro.CodigoTerminal);
            
        }

        public IList<QuotaPlanejadoRealizadoVm> PlanejadoRealizado(RelatorioAgendamentoFiltroVm filtro)
        {
            AplicaFiltros(filtro);

            var quotas = (from quota in _quotas.GetQuery()
                          group quota by new {quota.CodigoTerminal,quota.Fornecedor.Codigo, quota.Fornecedor.Nome, quota.FluxoDeCarga}
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
                                         NomeDoFornecedor = x.Key.Codigo + " - " + x.Key.Nome,
                                         FluxoDeCarga = x.Key.FluxoDeCarga.Descricao(),
                                         Quota = x.PlanejadoTotal,
                                         PesoRealizado = x.RealizadoTotal
                                     }).ToList();

        }

        public IList<QuotaCadastroVm> ListagemDeQuotas(RelatorioAgendamentoFiltroVm filtro)
        {
            AplicaFiltros(filtro);
            var quotas = (from quota in _quotas.GetQuery()
                          select new
                              {
                                  Terminal = quota.CodigoTerminal,
                                  Data = quota.Data.ToShortDateString(),
                                  quota.FluxoDeCarga,
                                  Fornecedor = quota.Fornecedor.Codigo + " - " + quota.Fornecedor.Nome,
                                  Peso = quota.PesoTotal
                              }).ToList();

            return quotas.Select(x =>
                                 new QuotaCadastroVm
                                     {
                                         Terminal = x.Terminal ,
                                         Data = x.Data ,
                                         Fornecedor = x.Fornecedor ,
                                         FluxoDeCarga = x.FluxoDeCarga.Descricao() ,
                                         Peso =  x.Peso,
                                     }).ToList();
        }

        public IList<AgendamentoDeCargaRelatorioListarVm> ListagemDeAgendamentos(RelatorioAgendamentoFiltroVm filtro)
        {
            AplicaFiltros(filtro);
            string placa = filtro.Placa;
            if (!string.IsNullOrEmpty(placa))
            {
                placa = placa.Replace("-", "").ToLower();
            }

            var agendamentos = (from quota in _quotas.GetQuery()
                                from agendamento in quota.Agendamentos
                                where
                                    (string.IsNullOrEmpty(filtro.Placa) ||
                                     agendamento.Placa.Replace("-", "").ToLower() == placa)
                                select new
                                    {
                                        Terminal = quota.CodigoTerminal,
                                        quota.Data,
                                        Fornecedor = quota.Fornecedor.Codigo + " - " + quota.Fornecedor.Nome,
                                        quota.FluxoDeCarga,
                                        quota.Material,
                                        agendamento.Placa,
                                        Peso = agendamento.PesoTotal
                                    }
                               ).ToList();

            return agendamentos.Select(x =>
                                       new AgendamentoDeCargaRelatorioListarVm
                                           {
                                               Terminal = x.Terminal,
                                               Data = x.Data.ToShortDateString(),
                                               Fornecedor = x.Fornecedor,
                                               FluxoDeCarga = x.FluxoDeCarga.Descricao(),
                                               Material = x.Material.Descricao(),
                                               Placa = x.Placa,
                                               Peso = x.Peso
                                           }).ToList();
        }
    }
}