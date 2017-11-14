using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeMaterial : IConsultaProcessoDeCotacaoDeMaterial
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IProcessoCotacaoIteracoesUsuario _iteracoesUsuario;
        private readonly IBuilder<Fornecedor, FornecedorCadastroVm> _builderFornecedor;


        public ConsultaProcessoDeCotacaoDeMaterial(IProcessosDeCotacao processosDeCotacao, IBuilder<Fornecedor, FornecedorCadastroVm> builderFornecedor
            , IProcessoCotacaoIteracoesUsuario iteracoesUsuario)
        {
            _processosDeCotacao = processosDeCotacao;
            _builderFornecedor = builderFornecedor;
            _iteracoesUsuario = iteracoesUsuario;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro)
        {
            _processosDeCotacao.FiltraPorTipo(Enumeradores.TipoDeCotacao.Material);
            if (filtro.CodigoFornecedor != null)
            {
                _processosDeCotacao
                    .DesconsideraNaoIniciados()
                    .FiltraPorFornecedor(filtro.CodigoFornecedor);

            }

            _processosDeCotacao.CodigoDoProdutoContendo(filtro.CodigoProduto)
                .DescricaoDoProdutoContendo(filtro.DescricaoProduto);

            if (filtro.CodigoStatusProcessoCotacao.HasValue)
            {
                _processosDeCotacao.FiltraPorStatus((Enumeradores.StatusProcessoCotacao) Enum.Parse(typeof (Enumeradores.StatusProcessoCotacao), Convert.ToString(filtro.CodigoStatusProcessoCotacao.Value)));
            }

            var query = _processosDeCotacao.GetQuery();

            query = query.OrderBy(x => x.Status);

            var quantidadeDeRegistros = query.Count();

            var processosListados = query.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).ToList();

            var registros = (from x in processosListados
                             let material = String.Join(", ", x.Itens.Select(i => i.Produto.Descricao))
                                 select
                                 new ProcessoCotacaoListagemVm()
                                 {
                                     Id = x.Id,
                                     //CodigoMaterial = x.CodigoMaterial,
                                     Material = string.IsNullOrEmpty(material) ? "Sem Materiais": material,
                                     DataTermino = x.DataLimiteDeRetorno.HasValue ? x.DataLimiteDeRetorno.Value.ToShortDateString(): "",
                                     //Quantidade = x.Quantidade,
                                     Status = x.Status.Descricao(),
                                     //UnidadeDeMedida = x.UnidadeDeMedida
                                 }).Cast<ListagemVm>().ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = quantidadeDeRegistros,
                    Registros = registros
                };

            return kendoGridVm;

        }

        public KendoGridVm ListarPorFornecedor(PaginacaoVm paginacao, ProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            AplicarFiltros(filtro);

            _processosDeCotacao
                .DesconsideraNaoIniciados();

            var query = (from processo in _processosDeCotacao.GetQuery()
                orderby processo.Id descending
                let p = (ProcessoDeCotacaoDeFrete)processo
                from itemDeCotacao in processo.Itens
                from fornecedorParticipante in p.FornecedoresParticipantes
                where fornecedorParticipante.Fornecedor.Codigo == filtro.CodigoFornecedor
                select new
                {
                    CodigoMaterial = itemDeCotacao.Produto.Codigo,
                    Material = itemDeCotacao.Produto.Descricao,
                    DataTermino = p.DataLimiteDeRetorno,
                    p.Id,
                    itemDeCotacao.Quantidade,
                    p.Status,
                    UnidadeDeMedida = itemDeCotacao.UnidadeDeMedida.Descricao,
                    Terminal = p.Terminal.Nome,
                    fornecedorParticipante.Resposta
                }

            );

            var quantidadeDeRegistros = query.Count();

            var registros = query.Skip(paginacao.Skip).Take(paginacao.Take).ToList()
                .Select(x => new CotacaoListagemVm()
                {
                    Id = x.Id,
                    CodigoMaterial = x.CodigoMaterial,
                    Material = x.Material,
                    DataTermino = x.DataTermino?.ToShortDateString() ?? "",
                    Quantidade = x.Quantidade,
                    Status = x.Status.Descricao(),
                    UnidadeDeMedida = x.UnidadeDeMedida,
                    Terminal = x.Terminal,
                    Resposta = x.Resposta.Descricao()
                }).Cast<ListagemVm>().ToList();

            var kendoGridVm = new KendoGridVm()
            {
                QuantidadeDeRegistros = quantidadeDeRegistros,
                Registros = registros
            };

            return kendoGridVm;


        }
        public ProcessoCotacaoMaterialCadastroVm ConsultaProcesso(int idProcessoCotacaoMaterial)
        {
             _processosDeCotacao.BuscaPorId(idProcessoCotacaoMaterial);
            var processoDeCotacao = (from p in _processosDeCotacao.GetQuery()
                                     let processo = (ProcessoDeCotacaoDeMaterial) p
                                     select new
                                         {
                                             processo.Id,
                                             DataTerminoLeilao = p.DataLimiteDeRetorno,
                                             processo.Status,
                                             processo.Requisitos,
                                             //processo.RequisicaoDeCompra.Numero,
                                             //processo.RequisicaoDeCompra.NumeroItem,
                                             //processo.RequisicaoDeCompra.Centro,
                                             //CodigoMaterial = processo.Produto.Codigo,
                                             //Material = processo.Produto.Descricao,
                                             //processo.RequisicaoDeCompra.Descricao,
                                             //processo.Quantidade,
                                             //processo.RequisicaoDeCompra.DataDeLiberacao,
                                             //processo.RequisicaoDeCompra.DataDeRemessa,
                                             //processo.RequisicaoDeCompra.DataDeSolicitacao,
                                             //FornecedorPretendido = processo.RequisicaoDeCompra.FornecedorPretendido.Nome,
                                             //Criador = processo.RequisicaoDeCompra.Criador.Nome, 
                                             //processo.RequisicaoDeCompra.Requisitante,
                                             //processo.RequisicaoDeCompra.UnidadeMedida
                                         }).Single();

            return new ProcessoCotacaoMaterialCadastroVm()
                {
                    Id = processoDeCotacao.Id,
                    DataLimiteRetorno = processoDeCotacao.DataTerminoLeilao.HasValue ? processoDeCotacao.DataTerminoLeilao.Value.ToShortDateString(): null,
                    DescricaoStatus = processoDeCotacao.Status.Descricao(),
                    Requisitos =  processoDeCotacao.Requisitos,
                    PermiteAlterarFornecedores = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.NaoIniciado,
                    PermiteFecharProcesso = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.Aberto,
                    PermiteSalvar = processoDeCotacao.Status != Enumeradores.StatusProcessoCotacao.Fechado,
                    PermitirAbrirProcesso = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.NaoIniciado,
                    PermiteSelecionarCotacoes = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.Aberto,
                };
        }

        public KendoGridVm FornecedoresParticipantes(int idProcessoCotacao)
        {
            IList<Fornecedor> fornecedoresParticipantes = (from p in
                                                               _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
                                                           from fp in p.FornecedoresParticipantes
                                                           select fp.Fornecedor).ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = fornecedoresParticipantes.Count,
                    Registros = _builderFornecedor.BuildList(fornecedoresParticipantes).Cast<ListagemVm>().ToList()
                };

            return kendoGridVm;

        }

        public IList<CotacaoMaterialSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            var retorno = new List<CotacaoMaterialSelecionarVm>();
            ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
            {
                var cotacaoSelecionarVm = new CotacaoMaterialSelecionarVm
                    {
                        CodigoFornecedor = fornecedorParticipante.Fornecedor.Codigo,
                        Fornecedor = fornecedorParticipante.Fornecedor.Nome
                    };
                retorno.Add(cotacaoSelecionarVm);

                if (fornecedorParticipante.Cotacao == null)
                {
                    continue;
                }

                var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao.CastEntity();

                cotacaoSelecionarVm.IdCotacao = cotacao.Id;
                cotacaoSelecionarVm.CondicaoDePagamento = cotacao.CondicaoDePagamento.Descricao;
                cotacaoSelecionarVm.Incoterm = cotacao.Incoterm.Descricao;

                var cotacaoItem = (CotacaoMaterialItem)cotacao.Itens.SingleOrDefault(x => x.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem);

                if (cotacaoItem == null)
                {
                    continue;
                }

                cotacaoSelecionarVm.QuantidadeAdquirida = cotacaoItem.QuantidadeAdquirida;
                cotacaoSelecionarVm.CodigoIva = cotacaoItem.Iva != null ? cotacaoItem.Iva.Codigo : null;
                cotacaoSelecionarVm.Preco = cotacaoItem.Preco;
                cotacaoSelecionarVm.ValorComImpostos = cotacaoItem.ValorComImpostos;
                cotacaoSelecionarVm.Custo = cotacaoItem.Custo;
                cotacaoSelecionarVm.Selecionada = cotacaoItem.Selecionada;

                Imposto imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.Icms);
                cotacaoSelecionarVm.ValorIcms = imposto != null ? imposto.Valor : (decimal?) null;

                imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.IcmsSubstituicao);
                cotacaoSelecionarVm.ValorIcmsSt = imposto != null ? imposto.Valor : (decimal?)null;

                imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.Ipi);
                cotacaoSelecionarVm.ValorIpi = imposto != null ? imposto.Valor : (decimal?)null;

                imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.PisCofins);
                cotacaoSelecionarVm.ValorPisCofins = imposto != null ? imposto.Valor : (decimal?)null; ;

            }

            return retorno;
        }

        public IList<FornecedorCotacaoVm> CotacoesDetalhadaDosFornecedores(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);
            //return (from pc in _processosDeCotacao.GetQuery()
            //        from fp in pc.FornecedoresParticipantes
            //        from cotacaoItem in fp.Cotacao.Itens
            //        where cotacaoItem.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem
            //        select new FornecedorCotacaoVm
            //            {
            //                Codigo = fp.Fornecedor.Codigo,
            //                PrecoInicial = cotacaoItem.PrecoInicial,
            //                PrecoFinal = cotacaoItem.Preco,
            //                QuantidadeAdquirida = cotacaoItem.QuantidadeAdquirida ?? 0,
            //                Selecionada = cotacaoItem.Selecionada,
            //                //Precos = (from historico in cotacaoItem.HistoricosDePreco select historico.Valor).ToArray()
            //                //Precos =  cotacaoItem.HistoricosDePreco.Select(x => x.Valor).ToList()
            //                Precos = (from historico in cotacaoItem.HistoricosDePreco orderby historico.DataHora ascending select historico.Valor).ToList()
            //            }).ToArray();
            var cotacoes = (from pc in _processosDeCotacao.GetQuery()
                    from fp in pc.FornecedoresParticipantes
                    from cotacaoItem in fp.Cotacao.Itens
                    from historico in cotacaoItem.HistoricosDePreco
                    where cotacaoItem.ProcessoDeCotacaoItem.Id == idProcessoCotacaoItem
                    orderby historico.DataHora, historico.Id
                    select new
                        {
                            fp.Fornecedor.Codigo,
                            cotacaoItem.PrecoInicial,
                            PrecoFinal = cotacaoItem.Preco,
                            QuantidadeAdquirida = cotacaoItem.QuantidadeAdquirida ?? 0,
                            cotacaoItem.Selecionada,
                            //Precos = (from historico in cotacaoItem.HistoricosDePreco select historico.Valor).ToArray()
                            //Precos =  cotacaoItem.HistoricosDePreco.Select(x => x.Valor).ToList()
                            Historico = historico.Valor
                        }).ToList();

            return (from r in cotacoes
             group r by new {r.Codigo, r.PrecoInicial, r.PrecoFinal, r.QuantidadeAdquirida, r.Selecionada}
             into grupo
             select new FornecedorCotacaoVm
                 {
                     Codigo = grupo.Key.Codigo,
                     PrecoInicial = grupo.Key.PrecoInicial,
                     PrecoFinal = grupo.Key.PrecoFinal,
                     QuantidadeAdquirida = grupo.Key.QuantidadeAdquirida,
                     Selecionada = grupo.Key.Selecionada,
                     Precos = grupo.Select(x => x.Historico).ToArray()

                 }).ToList();
        }

        public KendoGridVm CotacoesDosFornecedoresResumido(int idProcessoCotacao)
        {
            
            //var queryIteracoesUsuario = iteracoesUsuario.GetQuery();

            //var queryfp = (from p in
            //                   _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
            //               from fp in p.FornecedoresParticipantes
            //               select fp);

            //List<ProcessoCotacaoFornecedorVm> fornecedoresParticipantes =
            //    (from fp in queryfp
            //     join iu in queryIteracoesUsuario on fp.Id equals iu.IdFornecedorParticipante
            //         into joinedTable
            //     from iu in joinedTable.DefaultIfEmpty()
            //     select new ProcessoCotacaoFornecedorVm
            //         {
            //             IdFornecedorParticipante = fp.Id,
            //             Codigo = fp.Fornecedor.Codigo,
            //             Nome = fp.Fornecedor.Nome,
            //             Selecionado = (fp.Cotacao != null && fp.Cotacao.Selecionada ? "Sim" : "Não"),
            //             ValorLiquido = (fp.Cotacao != null ? fp.Cotacao.ValorLiquido : (decimal?)null),
            //             ValorComImpostos = (fp.Cotacao != null ? fp.Cotacao.ValorComImpostos : (decimal?)null),
            //             VisualizadoPeloFornecedor = iu != null && iu.VisualizadoPeloFornecedor ? "Sim" : "Não"
            //         }).ToList();

            List<FornecedorParticipante> fornecedoresParticipantes = (from p in
                                                                          _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
                                                                      from fp in p.FornecedoresParticipantes
                                                                      select fp).ToList();


            //List<ProcessoCotacaoFornecedorVm> fornecedoresParticipantes = (from p in
            //                                                              _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
            //                                                          from fp in p.FornecedoresParticipantes
            //                                                          join it in queryIteracoesUsuario on fp.Id equals it.IdFornecedorParticipante
            //                                                          select new ProcessoCotacaoFornecedorVm
            //                                                              {
            //                                                                    IdFornecedorParticipante = fp.Id,
            //                                                                    Codigo = fp.Fornecedor.Codigo ,
            //                                                                    Nome =  fp.Fornecedor.Nome,
            //                                                                    Selecionado = (fp.Cotacao != null && fp.Cotacao.Selecionada ? "Sim" : "Não") ,
            //                                                                    ValorLiquido = (fp.Cotacao != null ? fp.Cotacao.ValorLiquido : (decimal?) null),
            //                                                                    ValorComImpostos = (fp.Cotacao != null ? fp.Cotacao.ValorComImpostos : (decimal?)null),
            //                                                                   // VisualizadoPeloFornecedor = it.VisualizadoPeloFornecedor ? "Sim": "Não"
            //                                                              }).ToList();

            //List<ProcessoCotacaoFornecedorVm> fornecedoresParticipantes = queryParticipantes.Join( queryIteracoesUsuario, p => p.FornecedoresParticipantes.Select(fp => fp.Id),iu => iu.IdFornecedorParticipante )

            var registros = new List<ProcessoCotacaoFornecedorVm>();

            foreach (var fornecedorParticipante in fornecedoresParticipantes)
            {
                ProcessoCotacaoIteracaoUsuario iteracaoUsuario = _iteracoesUsuario.BuscaPorIdParticipante(fornecedorParticipante.Id);
                registros.Add(new ProcessoCotacaoFornecedorVm
                    {
                        IdFornecedorParticipante = fornecedorParticipante.Id,
                        Codigo = fornecedorParticipante.Fornecedor.Codigo,
                        Nome = fornecedorParticipante.Fornecedor.Nome,
                        Selecionado =
                            (fornecedorParticipante.Cotacao != null && fornecedorParticipante.Cotacao.Itens.Any(x => x.Selecionada)
                                 ? "Sim"
                                 : "Não"),
                        //ValorLiquido =
                        //    (fornecedorParticipante.Cotacao != null
                        //         ? fornecedorParticipante.Cotacao.ValorLiquido
                        //         : (decimal?) null),
                        //ValorComImpostos =
                        //    (fornecedorParticipante.Cotacao != null
                        //         ? fornecedorParticipante.Cotacao.ValorComImpostos
                        //         : (decimal?) null),
                        //QuantidadeDisponivel = fornecedorParticipante.Cotacao != null ? fornecedorParticipante.Cotacao.QuantidadeDisponivel : (decimal?) null,
                        VisualizadoPeloFornecedor = iteracaoUsuario != null && iteracaoUsuario.VisualizadoPeloFornecedor ? "Sim" : "Não"
                    });
            }


            var kendoGridVm = new KendoGridVm()
            {
                QuantidadeDeRegistros = registros.Count,
                //Registros = _builderProcessoCotacao.BuildList(fornecedoresParticipantes).Cast<ListagemVm>().ToList()
                Registros = registros.Cast<ListagemVm>().ToList()
            };

            return kendoGridVm;
        }

        public KendoGridVm ListarItens(int idProcessoCotacao)
        {
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);
            //var requisicoes = (from pc in _processosDeCotacao.GetQuery()
            //                   from item in pc.Itens
            //                   let itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item
            //                   select itemMaterial.RequisicaoDeCompra).ToList();

            var itens = (from pc in _processosDeCotacao.GetQuery()
                               from item in pc.Itens
                               let itemMaterial = (ProcessoDeCotacaoDeMaterialItem) item
                               select new ProcessoDeCotacaoDeMaterialItemVm
                                   {
                                       Id = pc.Id,
                                       IdProcessoCotacaoItem = itemMaterial.Id ,
                                       NumeroRequisicao = itemMaterial.RequisicaoDeCompra.Numero,
                                       NumeroItem = itemMaterial.RequisicaoDeCompra.NumeroItem ,
                                       Material = itemMaterial.Produto.Descricao ,
                                       Quantidade = itemMaterial.Quantidade,
                                       UnidadeMedida = itemMaterial.UnidadeDeMedida.Descricao ,
                                       DataDeSolicitacao = itemMaterial.RequisicaoDeCompra.DataDeSolicitacao.ToShortDateString() ,
                                       CodigoGrupoDeCompra = itemMaterial.RequisicaoDeCompra.CodigoGrupoDeCompra ,
                                   }).ToList();


            return new KendoGridVm
                {
                    QuantidadeDeRegistros = itens.Count,
                    Registros = itens.Cast<ListagemVm>().ToList()
                };
        }

        public string[] CodigoDosProdutos(int idProcessoCotacao)
        {
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);
            return (from pc in _processosDeCotacao.GetQuery()
             from item in pc.Itens
             select item.Produto.Codigo).Distinct().ToArray();
        }

        public FornecedorVm[] ListarFornecedores(int idProcessoCotacao)
        {
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);
            return (from pc in _processosDeCotacao.GetQuery()
                    from pf in pc.FornecedoresParticipantes
                    select new FornecedorVm
                    {
                        Codigo = pf.Fornecedor.Codigo,
                        Nome = pf.Fornecedor.Nome
                    }).ToArray();
        }

        private void AplicarFiltros(ProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            _processosDeCotacao.FiltraPorTipo(
                (Enumeradores.TipoDeCotacao)Enum.Parse(typeof(Enumeradores.TipoDeCotacao), Convert.ToString(filtro.TipoDeCotacao)));

            _processosDeCotacao.CodigoDoProdutoContendo(filtro.CodigoProduto)
                .DescricaoDoProdutoContendo(filtro.DescricaoProduto);

            if (filtro.CodigoStatusProcessoCotacao.HasValue)
            {
                _processosDeCotacao.FiltraPorStatus((Enumeradores.StatusProcessoCotacao)Enum.Parse(typeof(Enumeradores.StatusProcessoCotacao),
                    Convert.ToString(filtro.CodigoStatusProcessoCotacao.Value)));
            }
            else
            {
                _processosDeCotacao.DesconsideraCancelados();
            }

            //if (!string.IsNullOrEmpty(filtro.NumeroDoContrato))
            //{
            //    _processosDeCotacao.PertencentesAoContratoDeNumero(filtro.NumeroDoContrato);
            //}

            //if (!string.IsNullOrEmpty(filtro.NomeDoFornecedorDaMercadoria))
            //{
            //    _processosDeCotacao.NomeDoFornecedorDaMercadoriaContendo(filtro.NomeDoFornecedorDaMercadoria);
            //}

            //if (!string.IsNullOrEmpty(filtro.CodigoDoMunicipioDeOrigem))
            //{
            //    _processosDeCotacao.ComOrigemNoMunicipio(filtro.CodigoDoMunicipioDeOrigem);
            //}

            //if (!string.IsNullOrEmpty(filtro.CodigoDoTerminal))
            //{
            //    _processosDeCotacao.DoTerminal(filtro.CodigoDoTerminal);
            //}


        }
    }
}