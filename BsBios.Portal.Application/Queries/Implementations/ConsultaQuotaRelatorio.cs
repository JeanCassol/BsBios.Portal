using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using NHibernate;

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

        private IQueryOver<Quota, Quota> ObtemQueryOverComFiltrosAplicados(RelatorioAgendamentoFiltroVm filtro)
        {
            IQueryOver<Quota, Quota> queryOver = _quotas.GetQueryOver();
            if (filtro.DataDe.HasValue)
            {
                queryOver = queryOver.Where(x => x.Data >= filtro.DataDe.Value);
            }
            if (filtro.DataAte.HasValue)
            {
                queryOver = queryOver.Where(x => x.Data <= filtro.DataAte.Value);
            }
            if (filtro.CodigoFluxoDeCarga.HasValue)
            {
                queryOver = queryOver.Where(x => x.FluxoDeCarga == (Enumeradores.FluxoDeCarga)
                    Enum.Parse(typeof(Enumeradores.FluxoDeCarga), Convert.ToString(filtro.CodigoFluxoDeCarga.Value))); 

            }
            if (!string.IsNullOrEmpty(filtro.CodigoFornecedor))
            {
                queryOver = queryOver.Where(x => x.Fornecedor.Codigo == filtro.CodigoFornecedor);
            }

            queryOver = queryOver.Where(x => x.Terminal.Codigo == filtro.CodigoTerminal);

            return queryOver;
        }

        public IList<QuotaPlanejadoRealizadoVm> PlanejadoRealizado(RelatorioAgendamentoFiltroVm filtro)
        {
            AplicaFiltros(filtro);

            var quotas = (from quota in _quotas.GetQuery()
                          group quota by new 
                          {
                              CodigoTerminal = quota.Terminal.Codigo,
                              quota.Fornecedor.Codigo, 
                              quota.Fornecedor.Nome, 
                              quota.FluxoDeCarga
                          }
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

        public IList<QuotaPlanejadoRealizadoPorDataVm> PlanejadoRealizadoPorData(RelatorioAgendamentoFiltroVm filtro)
        {

            //OBS: GROUP BY COM ORDER BY CAUSA UM ERRO UTILIZANDO IQUERYABLE (GetQuery).
            //Por isso tive que utilizar IQueryOver

            //var query = (from quota in _quotas.GetQuery()
            //              group quota by new { quota.CodigoTerminal, quota.Data, quota.Fornecedor.Codigo, quota.Fornecedor.Nome, quota.FluxoDeCarga }
            //                  into agrupador
            //                  select new
            //                  {
            //                      agrupador.Key,
            //                      PlanejadoTotal = agrupador.Sum(x => x.PesoTotal),
            //                      RealizadoTotal = agrupador.Sum(x => x.PesoRealizado)
            //                  }
            //                  ).ToList()
            //                  .OrderBy(x => new{x.Key.CodigoTerminal, x.Key.Data});


            IQueryOver<Quota, Quota> queryOver = ObtemQueryOverComFiltrosAplicados(filtro);
            Fornecedor fornec = null;
            QuotaPlanejadoRealizadoPorDataVm alias = null;
            queryOver = queryOver
                .JoinAlias(x => x.Fornecedor, () => fornec)
                .SelectList(list => list
                                             .SelectGroup(x => x.Terminal.Codigo).WithAlias(() => alias.CodigoTerminal)
                                             .SelectGroup(x => x.Data)
                                             .SelectGroup(x => x.Fornecedor.Codigo)
                                             .SelectGroup(x => fornec.Nome)
                                             .SelectGroup(x => x.FluxoDeCarga)
                                             .SelectSum(x => x.PesoTotal)
                                             .SelectSum(x => x.PesoRealizado)
                ).OrderBy(x => x.Terminal.Codigo).Asc.OrderBy(x => x.Data).Asc;

            //tive que utilizar um array de objetos porque na query que executa no banco ainda não tenho o método Descricao() do Enum.
            return queryOver
                .List<object[]>()
                .Select(properties => new QuotaPlanejadoRealizadoPorDataVm
                    {
                        CodigoTerminal = (string) properties[0],
                        Data =  ((DateTime) properties[1]).ToShortDateString(),
                        NomeDoFornecedor = ((string) properties[2]) + " - " + (string) properties[3],
                        FluxoDeCarga = ((Enumeradores.FluxoDeCarga) properties[4]).Descricao(),
                        Quota = (decimal) properties[5],
                        PesoRealizado = (decimal) properties[6]
                    }).ToList();

        }

        public IList<QuotaCadastroVm> ListagemDeQuotas(RelatorioAgendamentoFiltroVm filtro)
        {
            AplicaFiltros(filtro);
            var quotas = (from quota in _quotas.GetQuery()
                          orderby quota.Data
                          select new
                              {
                                  Terminal = quota.Terminal.Codigo,
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
                                orderby quota.Data
                                select new
                                    {
                                        Terminal = quota.Terminal.Codigo,
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