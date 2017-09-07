using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using BsBios.Portal.ViewModel;
using NHibernate;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaQuotaRelatorio : IConsultaQuotaRelatorio
    {
        private readonly Quotas _quotas;

        public ConsultaQuotaRelatorio(Quotas quotas)
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

            if (!string.IsNullOrEmpty(filtro.CodigoTerminal))
            {
                queryOver = queryOver.Where(x => x.Terminal.Codigo == filtro.CodigoTerminal);
            }

            return queryOver;
        }

        public RelatorioDeQuotaPlanejadoVersusRealizadoVm PlanejadoRealizado(RelatorioAgendamentoFiltroVm filtro)
        {
            AplicaFiltros(filtro);

            var quotas = (from quota in _quotas.GetQuery()
                          group quota by new 
                          {
                              CodigoTerminal = quota.Terminal.Codigo,
                              quota.Fornecedor.Codigo, 
                              quota.Fornecedor.Nome, 
                              quota.FluxoDeCarga,
                              Material = quota.Material.Descricao
                          }
                          into agrupador
                          select new
                              {
                                  agrupador.Key,
                                  PlanejadoTotal = agrupador.Sum(x => x.PesoTotal),
                                  RealizadoTotal = agrupador.Sum(x => x.PesoRealizado),
                                  NaoRealizadoTotal = agrupador.Sum(x => x.PesoAgendado - x.PesoRealizado)
                              }).ToList();


            List<QuotaPlanejadoRealizadoVm> registros = quotas.Select(x =>
                new QuotaPlanejadoRealizadoVm
                {
                    CodigoTerminal = x.Key.CodigoTerminal,
                    NomeDoFornecedor = x.Key.Codigo + " - " + x.Key.Nome,
                    FluxoDeCarga = x.Key.FluxoDeCarga.Descricao(),
                    Material =  x.Key.Material,
                    Quota = x.PlanejadoTotal,
                    PesoRealizado = x.RealizadoTotal,
                    PesoNaoRealizado = x.NaoRealizadoTotal
                }).ToList();

            return new RelatorioDeQuotaPlanejadoVersusRealizadoVm
            {
                Quotas = registros,
                Total = new QuotaPlanejadoRealizadoTotalVm
                {
                    Quota =  registros.Sum(r => r.Quota),
                    PesoRealizado = registros.Sum(r => r.PesoRealizado),
                    PesoNaoRealizado = registros.Sum(r => r.PesoNaoRealizado)

                }
            };

        }

        public RelatorioDeQuotaPlanejadoVersusRealizadoPorDataVm PlanejadoRealizadoPorData(RelatorioAgendamentoFiltroVm filtro)
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
            MaterialDeCarga materialDeCarga = null;
            QuotaPlanejadoRealizadoPorDataVm alias = null;
            queryOver = queryOver
                .JoinAlias(x => x.Fornecedor, () => fornec)
                .JoinAlias(x => x.Material, () => materialDeCarga)
                .SelectList(list => list
                                             .SelectGroup(x => x.Terminal.Codigo).WithAlias(() => alias.CodigoTerminal)
                                             .SelectGroup(x => x.Data)
                                             .SelectGroup(x => x.Fornecedor.Codigo)
                                             .SelectGroup(x => fornec.Nome)
                                             .SelectGroup(x => x.FluxoDeCarga)
                                             .SelectGroup(x => materialDeCarga.Descricao)
                                             .SelectSum(x => x.PesoTotal)
                                             .SelectSum(x => x.PesoRealizado)
                                             .SelectSum(x => x.PesoAgendado)
                ).OrderBy(x => x.Terminal.Codigo).Asc.OrderBy(x => x.Data).Asc;

            //tive que utilizar um array de objetos porque na query que executa no banco ainda não tenho o método Descricao() do Enum.
            List<QuotaPlanejadoRealizadoPorDataVm> quotas = queryOver
                .List<object[]>()
                .Select(properties => new QuotaPlanejadoRealizadoPorDataVm
                {
                    CodigoTerminal = (string) properties[0],
                    Data =  ((DateTime) properties[1]).ToShortDateString(),
                    NomeDoFornecedor = ((string) properties[2]) + " - " + (string) properties[3],
                    FluxoDeCarga = ((Enumeradores.FluxoDeCarga) properties[4]).Descricao(),
                    Material = (string)properties[5],
                    Quota = (decimal) properties[6],
                    PesoRealizado = (decimal) properties[7],
                    PesoNaoRealizado = (decimal)properties[8] - (decimal)properties[7] //PesoNaoRealizado = PesoAgendado - PesoRealizado
                }).ToList();

            return new RelatorioDeQuotaPlanejadoVersusRealizadoPorDataVm
            {
                Quotas = quotas,
                Total = new QuotaPlanejadoRealizadoTotalVm
                {
                    Quota = quotas.Sum(q => q.Quota),
                    PesoRealizado = quotas.Sum(q => q.PesoRealizado),
                    PesoNaoRealizado = quotas.Sum(q => q.PesoNaoRealizado)
                }
            };
        }

        public IList<QuotaListagemVm> ListagemDeQuotas(RelatorioAgendamentoFiltroVm filtro)
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
                                  Material = quota.Material.Descricao,
                                  Peso = quota.PesoTotal,
                                  quota.PesoAgendado
                              }).ToList();

            return quotas.Select(x =>
                                 new QuotaListagemVm
                                     {
                                         Terminal = x.Terminal ,
                                         Data = x.Data ,
                                         Fornecedor = x.Fornecedor ,
                                         FluxoDeCarga = x.FluxoDeCarga.Descricao() ,
                                         Material = x.Material,
                                         Peso =  x.Peso,
                                         PesoAgendado = x.PesoAgendado
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
                                        Material = quota.Material.Descricao,
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
                                               Material = x.Material,
                                               Placa = x.Placa,
                                               Peso = x.Peso
                                           }).ToList();
        }
    }
}