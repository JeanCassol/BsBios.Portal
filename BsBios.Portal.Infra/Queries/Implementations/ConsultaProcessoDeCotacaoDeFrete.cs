using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Implementations
{
    public class ConsultaProcessoDeCotacaoDeFrete : IConsultaProcessoDeCotacaoDeFrete
    {
        private readonly IProcessosDeCotacaoDeFrete _processosDeCotacao;
        private readonly IProcessoCotacaoIteracoesUsuario _iteracoesUsuario;


        public ConsultaProcessoDeCotacaoDeFrete(IProcessosDeCotacaoDeFrete processosDeCotacao, IProcessoCotacaoIteracoesUsuario iteracoesUsuario)
        {
            _processosDeCotacao = processosDeCotacao;
            _iteracoesUsuario = iteracoesUsuario;
        }


        public ProcessoCotacaoFreteCadastroVm ConsultaProcesso(int idProcessoCotacao)
        {
             
            var processoDeCotacao = (ProcessoDeCotacaoDeFrete)  _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
            var item = (ProcessoDeCotacaoDeFreteItem) processoDeCotacao.Itens.First();

            var processoCotacaoFreteCadastroVm = new ProcessoCotacaoFreteCadastroVm()
            {
                Id = processoDeCotacao.Id,
                DataLimiteRetorno = processoDeCotacao.DataLimiteDeRetorno.Value.ToShortDateString(),
                DescricaoStatus = processoDeCotacao.Status.Descricao(),
                CodigoMaterial = item.Produto.Codigo,
                DescricaoMaterial = item.Produto.Descricao,
                QuantidadeMaterial = item.Quantidade,
                CodigoUnidadeMedida = item.UnidadeDeMedida.CodigoInterno,
                CodigoItinerario = processoDeCotacao.Itinerario.Codigo,
                DescricaoItinerario = processoDeCotacao.Itinerario.Descricao ,
                Requisitos = processoDeCotacao.Requisitos ,
                NumeroDoContrato = processoDeCotacao.NumeroDoContrato ,
                DataValidadeCotacaoInicial = processoDeCotacao.DataDeValidadeInicial.ToShortDateString() ,
                DataValidadeCotacaoFinal = processoDeCotacao.DataDeValidadeFinal.ToShortDateString() ,

                Cadencia = item.Cadencia,
                Classificacao = processoDeCotacao.Classificacao,
                CodigoDoFornecedorDaMercadoria = processoDeCotacao.FornecedorDaMercadoria?.Codigo,
                FornecedorDaMercadoria = processoDeCotacao.FornecedorDaMercadoria?.Nome,
                CodigoDoMunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem?.Codigo,
                NomeDoMunicipioDeOrigem = processoDeCotacao.MunicipioDeOrigem != null ? $"{processoDeCotacao.MunicipioDeOrigem.Nome}/{processoDeCotacao.MunicipioDeOrigem.UF}": null,
                CodigoDoMunicipioDeDestino = processoDeCotacao.MunicipioDeDestino?.Codigo,
                NomeDoMunicipioDeDestino = processoDeCotacao.MunicipioDeDestino != null ? $"{processoDeCotacao.MunicipioDeDestino.Nome}/{processoDeCotacao.MunicipioDeDestino.UF}": null,
                CodigoDoDeposito = processoDeCotacao.Deposito?.Codigo,
                Deposito = processoDeCotacao.Deposito?.Nome,
                CodigoDoTerminal = processoDeCotacao.Terminal.Codigo,
                TipoDePreco = (int)item.TipoDePreco,
                ValorPrevisto = item.ValorPrevisto,

                PermiteAlterarFornecedores = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.NaoIniciado,
                PermiteFecharProcesso = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.Aberto,
                PermiteSalvar = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.NaoIniciado,
                PermitirAbrirProcesso = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.NaoIniciado,
                PermiteSelecionarCotacoes = processoDeCotacao.Status == Enumeradores.StatusProcessoCotacao.Aberto,

            };

            switch (item.TipoDePreco)
            {
                case Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorFechado:
                    //processoCotacaoFreteCadastroVm.LabelDoTipoDePreco = "Valor Fechado";
                    processoCotacaoFreteCadastroVm.ValorDoTipoDePreco = item.ValorFechado;
                    break;
                case Enumeradores.TipoDePrecoDoProcessoDeCotacao.ValorMaximo:
                    //processoCotacaoFreteCadastroVm.LabelDoTipoDePreco = "Valor Máximo";
                    processoCotacaoFreteCadastroVm.ValorDoTipoDePreco = item.ValorMaximo;
                    break;
            }
            return processoCotacaoFreteCadastroVm;
        }

        public IList<CotacaoSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao)
        {
            //var retorno = new List<CotacaoSelecionarVm>();
            _processosDeCotacao.BuscaPorId(idProcessoCotacao);

            var retorno = new List<CotacaoSelecionarVm>();

            var fornecedoresParticipantes = (from pc in _processosDeCotacao.GetQuery()
                                             from fp in pc.FornecedoresParticipantes
                                            select fp).ToList();

            foreach (var fornecedorParticipante in fornecedoresParticipantes)
            {
                var cotacaoSelecionarVm = new CotacaoSelecionarVm
                    {
                        CodigoFornecedor = fornecedorParticipante.Fornecedor.Codigo,
                        Fornecedor = fornecedorParticipante.Fornecedor.Nome,
                        Cnpj = fornecedorParticipante.Fornecedor.Cnpj
                    };
                retorno.Add(cotacaoSelecionarVm);

                if (fornecedorParticipante.Cotacao == null)
                {
                    continue;
                }

                var cotacao = (CotacaoDeFrete) fornecedorParticipante.Cotacao.CastEntity();
                cotacaoSelecionarVm.IdCotacao = cotacao.Id;

                var cotacaoItem = (CotacaoFreteItem) cotacao.Itens.SingleOrDefault();

                if (cotacaoItem == null)
                {
                    continue;
                }

                cotacaoSelecionarVm.QuantidadeAdquirida = cotacaoItem.QuantidadeAdquirida;
                cotacaoSelecionarVm.QuantidadeDisponivel = cotacaoItem.QuantidadeDisponivel;
                cotacaoSelecionarVm.ValorComImpostos = cotacaoItem.ValorComImpostos;
                cotacaoSelecionarVm.Selecionada = cotacaoItem.Selecionada;
                cotacaoSelecionarVm.Cadencia = cotacaoItem.Cadencia;
                cotacaoSelecionarVm.ObservacaoDoFornecedor = cotacaoItem.Observacoes;
                cotacaoSelecionarVm.PermiteSelecionar = fornecedorParticipante.Resposta == Enumeradores.RespostaDaCotacao.Aceito;
            }

            return retorno;
        }

        public KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            _processosDeCotacao.FiltraPorTipo(Enumeradores.TipoDeCotacao.Frete);
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
                _processosDeCotacao.FiltraPorStatus(
                    (Enumeradores.StatusProcessoCotacao)
                    Enum.Parse(typeof (Enumeradores.StatusProcessoCotacao),
                               Convert.ToString(filtro.CodigoStatusProcessoCotacao.Value)));
            }

            if (!string.IsNullOrEmpty(filtro.NumeroDoContrato))
            {
                _processosDeCotacao.PertencentesAoContratoDeNumero(filtro.NumeroDoContrato);
                
            }
            if (!string.IsNullOrEmpty(filtro.NomeDoFornecedorDaMercadoria))
            {
                _processosDeCotacao.NomeDoFornecedorDaMercadoriaContendo(filtro.NomeDoFornecedorDaMercadoria);
                
            }
            if (!string.IsNullOrEmpty(filtro.CodigoDoMunicipioDeOrigem))
            {
                _processosDeCotacao.ComOrigemNoMunicipio(filtro.CodigoDoMunicipioDeOrigem);
                
            }

            if (!string.IsNullOrEmpty(filtro.CodigoDoTerminal))
            {
                _processosDeCotacao.DoTerminal(filtro.CodigoDoTerminal);
            }

            var query = (from p in _processosDeCotacao.GetQuery()
                         from item in p.Itens
                         orderby p.Status
                         select new
                             {
                                 CodigoMaterial = item.Produto.Codigo,
                                 Material = item.Produto.Descricao,
                                 DataTermino = p.DataLimiteDeRetorno,
                                 Id = p.Id,
                                 Quantidade = item.Quantidade,
                                 Status = p.Status,
                                 UnidadeDeMedida = item.UnidadeDeMedida.Descricao
                             }
                        );

            var quantidadeDeRegistros = query.Count();

            var registros = query.Skip(paginacaoVm.Skip).Take(paginacaoVm.Take).ToList()
                                 .Select(x => new ProcessoCotacaoListagemVm()
                                     {
                                         Id = x.Id,
                                         CodigoMaterial = x.CodigoMaterial,
                                         Material = x.Material,
                                         DataTermino =
                                             x.DataTermino.HasValue ? x.DataTermino.Value.ToShortDateString() : "",
                                         Quantidade = x.Quantidade,
                                         Status = x.Status.Descricao(),
                                         UnidadeDeMedida = x.UnidadeDeMedida
                                     }).Cast<ListagemVm>().ToList();

            var kendoGridVm = new KendoGridVm()
                {
                    QuantidadeDeRegistros = quantidadeDeRegistros,
                    Registros = registros
                };

            return kendoGridVm;
        }

        public KendoGridVm CotacoesDosFornecedoresResumido(int idProcessoCotacao)
        {
            List<FornecedorParticipante> fornecedoresParticipantes = (from p in
                                                                          _processosDeCotacao.BuscaPorId(idProcessoCotacao).GetQuery()
                                                                      from fp in p.FornecedoresParticipantes
                                                                      select fp).ToList();


            var registros = new List<ProcessoCotacaoFornecedorVm>();

            foreach (var fornecedorParticipante in fornecedoresParticipantes)
            {
                ProcessoCotacaoIteracaoUsuario iteracaoUsuario = _iteracoesUsuario.BuscaPorIdParticipante(fornecedorParticipante.Id);
                var vm = new ProcessoCotacaoFornecedorVm
                    {
                        IdFornecedorParticipante = fornecedorParticipante.Id,
                        Codigo = fornecedorParticipante.Fornecedor.Codigo,
                        Nome = fornecedorParticipante.Fornecedor.Nome,
                        VisualizadoPeloFornecedor = iteracaoUsuario != null && iteracaoUsuario.VisualizadoPeloFornecedor ? "Sim" : "Não"
                    };

                if (fornecedorParticipante.Cotacao != null)
                {
                    var cotacaoItem = fornecedorParticipante.Cotacao.Itens.SingleOrDefault();
                    if (cotacaoItem != null)
                    {
                        vm.Selecionado = (cotacaoItem.Selecionada ? "Sim" : "Não");
                        vm.ValorLiquido = cotacaoItem.Preco;
                        vm.ValorComImpostos = cotacaoItem.ValorComImpostos;
                        vm.QuantidadeDisponivel = cotacaoItem.QuantidadeDisponivel;
                    }

                }

                registros.Add(vm);
            }

            var kendoGridVm = new KendoGridVm()
            {
                QuantidadeDeRegistros = registros.Count,
                Registros = registros.Cast<ListagemVm>().ToList()
            };

            return kendoGridVm;
        }

        public decimal CalcularQuantidadeContratadaNoProcessoDeCotacao(int idDoProcessoDeCotacao)
        {
            _processosDeCotacao.BuscaPorId(idDoProcessoDeCotacao);
            return (from processo in _processosDeCotacao.GetQuery()
                from participante in processo.FornecedoresParticipantes
                from cotacaoItem in participante.Cotacao.Itens
                where cotacaoItem.Selecionada
                select cotacaoItem.QuantidadeAdquirida
            ).Sum() ?? 0;

            //return _processosDeCotacao
            //    .BuscaPorId(idDoProcessoDeCotacao)
            //    .GetQuery()
            //    .Select(x => x.FornecedoresParticipantes.Where(fp => fp.Cotacao.Selecionada).Sum(fp => fp.Cotacao.QuantidadeAdquirida.Value)).SingleOrDefault();




        }
    }
}