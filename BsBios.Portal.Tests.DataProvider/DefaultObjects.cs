using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Tests.DataProvider
{
    public static class DefaultObjects
    {
        private static int _contadorFornecedores;
        private static int _contadorProdutos;
        private static int _contadorCondicoesDePagamento;
        private static int _contadorIvas;
        private static int _contadorIncoterms;
        private static int _contadorUsuarios;
        private static int _contadorRequisicaoCompra;
        private static int _contadorItinerario;
        private static int _contadorUnidadeMedida;

        private static string GeraCodigo(int contador, int tamanho)
        {
            string contadorToString = Convert.ToString(contador);
            if (contadorToString.Length == tamanho)
            {
                return contadorToString;
            }
            return new string('0', tamanho - contadorToString.Length) + contador;
        }

        public static string ObtemSufixoCodigoFornecedor(string codigoFornecedor)
        {
            return codigoFornecedor.Substring(9);
        }

        private static RequisicaoDeCompra ObtemRequisicaoDeCompraPadrao(Produto produto)
        {
            var usuarioCriador = ObtemUsuarioPadrao();
            var fornecedorPretendido = ObtemFornecedorPadrao();

            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            _contadorRequisicaoCompra++;

            string numeroRequisicao = GeraCodigo(_contadorRequisicaoCompra, 10);
            string numeroItem = GeraCodigo(_contadorRequisicaoCompra, 5);

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, "requisitante", fornecedorPretendido,
                dataDeRemessa, dataDeLiberacao, dataDeSolicitacao, "C001", ObtemUnidadeDeMedidaPadrao(), 1000,
                produto, "Requisição de Compra enviada pelo SAP", numeroItem, numeroRequisicao, "GC1", false);

            return requisicaoDeCompra;
            
        }

        public static RequisicaoDeCompra ObtemRequisicaoDeCompraPadrao()
        {
            return ObtemRequisicaoDeCompraPadrao(ObtemProdutoPadrao());
        }

        public static RequisicaoDeCompra ObtemRequisicaoDeCompraComId()
        {
            var usuarioCriador = ObtemUsuarioPadrao();
            var fornecedorPretendido = ObtemFornecedorPadrao();
            var material = ObtemProdutoPadrao();

            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            _contadorRequisicaoCompra++;

            string numeroRequisicao = GeraCodigo(_contadorRequisicaoCompra, 10);
            string numeroItem = GeraCodigo(_contadorRequisicaoCompra, 5);

            var requisicaoDeCompra = new RequisicaoDeCompraTeste(_contadorRequisicaoCompra, usuarioCriador, "requisitante", fornecedorPretendido,
                dataDeRemessa, dataDeLiberacao, dataDeSolicitacao, "C001", ObtemUnidadeDeMedidaPadrao(), 1000,
                material, "Requisição de Compra enviada pelo SAP", numeroItem, numeroRequisicao, "GC1", false);

            return requisicaoDeCompra;
        }


        public static RequisicaoDeCompra ObtemRequisicaoDeCompraSemRequisitanteEFornecedor()
        {
            var usuarioCriador = ObtemUsuarioPadrao();
            var material = ObtemProdutoPadrao();
            UnidadeDeMedida unidadeDeMedida = ObtemUnidadeDeMedidaPadrao();

            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, null, null,
                dataDeRemessa, dataDeLiberacao, dataDeSolicitacao, "C001", unidadeDeMedida, 1000,
                material, "Requisição de Compra enviada pelo SAP", "00001", "REQ0001", "GC1", false);

            return requisicaoDeCompra;
        }

        private static RequisicaoDeCompra ObtemRequisicaoDeMateriaPrima()
        {
            Produto produto = ObtemMateriaPrima();
            return ObtemRequisicaoDeCompraPadrao(produto);
        }

        private static Produto ObtemMateriaPrima()
        {
            return  ObtemProdutoPadrao("ROH");
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialNaoIniciado()
        {
            var requisicaoDeCompra = ObtemRequisicaoDeCompraPadrao();
            var processo = requisicaoDeCompra.GerarProcessoDeCotacaoDeMaterial();
            return processo;
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialAbertoPadrao()
        {
            return ObtemProcessoDeCotacaoDeMaterialAberto(ObtemUsuarioPadrao());
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialAberto(Usuario comprador)
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = ObtemProcessoDeCotacaoDeMaterialAtualizado();
            Fornecedor fornecedor = ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(comprador);
            return processoDeCotacao;
        }


        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialFechado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var cotacao = (CotacaoMaterial) processoDeCotacao.FornecedoresParticipantes.First().Cotacao;
            var processoDeCotacaoItem = processoDeCotacao.Itens.First();
            cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, 125, null, 100, DateTime.Today.AddMonths(1), "obs");
            processoDeCotacao.SelecionarCotacao(cotacao.Id, processoDeCotacaoItem.Id, 100, ObtemIvaPadrao());
            processoDeCotacao.Fechar();
            return processoDeCotacao;
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = ObtemProcessoDeCotacaoDeMaterialAbertoPadrao();
            var codigoFornecedor = processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
            CotacaoMaterial cotacao = processoDeCotacao.InformarCotacao(codigoFornecedor, ObtemCondicaoDePagamentoPadrao(), ObtemIncotermPadrao(), "Descrição do Incotem");
            var processoDeCotacaoItem = processoDeCotacao.Itens.First();
            cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, 125, 12, 100, DateTime.Today.AddMonths(1), "obs");
            return processoDeCotacao;            
        }

        public static Fornecedor ObtemFornecedorPadrao()
        {
            _contadorFornecedores++;
            var codigo = GeraCodigo(_contadorFornecedores, 10);
            var fornecedor = new Fornecedor(codigo, "FORNECEDOR " + codigo, 
                "fornecedor" + codigo + "@empresa.com.br","cnpj" + codigo, "municipio" + codigo, "uf", false, "Endereco" + codigo);
            return fornecedor;
        }

        public static Fornecedor ObtemTransportadoraPadrao()
        {
            _contadorFornecedores++;
            var codigo = GeraCodigo(_contadorFornecedores, 10);
            var fornecedor = new Fornecedor(codigo, "TRANSPORTADORA " + codigo,
                "fornecedor" + codigo + "@empresa.com.br", "cnpj" + codigo, "municipio" + codigo, "uf", true, "Endereco" + codigo);
            return fornecedor;
            
        }

        public static Usuario ObtemUsuarioPadrao()
        {
            _contadorUsuarios++;
            var codigo = GeraCodigo(_contadorUsuarios, 12);

            var usuario = new Usuario("Usuário " + codigo, codigo, 
                "usuario" + codigo + "@empresa.com.br");
            return usuario;
        }

        public static Usuario ObtemCompradorDeSuprimentos()
        {
            var usuario = ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.CompradorSuprimentos);
            return usuario;
        }


        public static Produto ObtemProdutoPadrao()
        {
            return ObtemProdutoPadrao("01");
        }

        private static Produto ObtemProdutoPadrao(string tipoDeProduto)
        {
            _contadorProdutos++;
            var codigo = GeraCodigo(_contadorProdutos, 18);
            var produto = new Produto(codigo, "PRODUTO " + codigo, tipoDeProduto);
            return produto;
        }


        public static Incoterm ObtemIncotermPadrao()
        {
            _contadorIncoterms ++;
            string codigo = GeraCodigo(_contadorIncoterms, 3);
            return new Incoterm(codigo, "INCOTERM " + codigo);
        }

        public static Iva ObtemIvaPadrao()
        {
            _contadorIvas++;
            var codigo = GeraCodigo(_contadorIvas, 2);
            return new Iva(codigo, "IVA " + codigo);
        }

        public static CondicaoDePagamento ObtemCondicaoDePagamentoPadrao()
        {
            _contadorCondicoesDePagamento++;
            var codigo = GeraCodigo(_contadorCondicoesDePagamento, 4);
            return new CondicaoDePagamento(codigo , "CONDIÇÃO " + codigo);
        }

        public static UsuarioConectado ObtemUsuarioConectado()
        {
            return new UsuarioConectado("comprador", "Usuário Comprador",new List<Enumeradores.Perfil>{Enumeradores.Perfil.CompradorSuprimentos});
        }

        public static Itinerario ObtemItinerarioPadrao()
        {
            _contadorItinerario++;
            string codigo = GeraCodigo(_contadorItinerario, 6);
            return new Itinerario(codigo, "ITINERÁRIO " + codigo);
        }

        public static UnidadeDeMedida ObtemUnidadeDeMedidaPadrao()
        {
            _contadorUnidadeMedida++;
            if (_contadorUnidadeMedida > 99)
            {
                _contadorUnidadeMedida = 0;
            }
            string codigo = GeraCodigo(_contadorUnidadeMedida, 2);
            return new UnidadeDeMedida("I" +  codigo, "E" + codigo, "Unidade de Medida " + codigo);
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialAtualizado()
        {
            ProcessoDeCotacaoDeMaterial processo = ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processo.Atualizar(DateTime.Today.AddDays(10), "Requisitos do Processo de Cotação de Materiais");
            return processo;
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialDeMateriaPrima()
        {
            RequisicaoDeCompra requisicaoDeCompra = ObtemRequisicaoDeMateriaPrima();
            ProcessoDeCotacaoDeMaterial processoDeCotacao = requisicaoDeCompra.GerarProcessoDeCotacaoDeMaterial();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(10),"requisitos");
            return processoDeCotacao;
        }

 public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFrete()
        {
            var processo= ObtemProcessoDeCotacaoDeFrete(ObtemMunicipioPadrao(), ObtemMunicipioPadrao());
            processo.AdicionarItem(ObtemProdutoPadrao(), 100, ObtemUnidadeDeMedidaPadrao());
			return processo;
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFrete(Municipio municipioOrigem, Municipio municipioDestino)
        {
            return new ProcessoDeCotacaoDeFrete(ObtemProdutoPadrao(), 100, ObtemUnidadeDeMedidaPadrao(),
                "Requisitos do Processo de Cotação de Frete", "1000", DateTime.Today.AddDays(10),
                DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), ObtemItinerarioPadrao(),
                ObtemFornecedorPadrao(), 100, true, municipioOrigem, municipioDestino, ObtemFornecedorPadrao(),
                ObtemTerminalPadrao(),100);
        }

        public static Terminal ObtemTerminalPadrao()
        {
            return new Terminal("1000", "Terminal de Passo Fundo","Passo Fundo");
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteCancelado(Municipio municipioOrigem, Municipio municipioDestino)
        {
            var processoDeCotacao = ObtemProcessoDeCotacaoDeFrete(municipioOrigem, municipioDestino);
            processoDeCotacao.Cancelar();
            return processoDeCotacao;
        }
        


        private static Municipio ObtemMunicipioPadrao()
        {
            return new Municipio("1000", "Torres");
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteSemNumeroDeContrato()
        {
            var processo = ProcessoDeCotacaoDeFrete(ObtemProdutoPadrao(), 100, ObtemUnidadeDeMedidaPadrao(),
                "Requisitos do Processo de Cotação de Frete", null, DateTime.Today.AddDays(10),
                DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), ObtemItinerarioPadrao(),
                ObtemFornecedorPadrao(), 100, true, ObtemMunicipioPadrao(), ObtemMunicipioPadrao(),ObtemFornecedorPadrao(),
                ObtemTerminalPadrao(),100);
            processo.AdicionarItem(ObtemProdutoPadrao(), 100, ObtemUnidadeDeMedidaPadrao());
            return processo;

        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComCadastrosExistentes()
        {
            var produto = new Produto("000000000000000007", "BORRA NEUTRALIZADA DE SOJA", "FERT");
            var unidadeDeMedida = new UnidadeDeMedida("TON", "TON", "Toneladas");
            var itinerario = new Itinerario("010330", "RS Rio Grande -> BA Formosa Do Rio Preto");

            var processo = new ProcessoDeCotacaoDeFrete(produto, 100, unidadeDeMedida, 
                "Requisitos do Processo de Cotação de Frete", null, DateTime.Today.AddDays(10),
                DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), itinerario,
                ObtemFornecedorPadrao(), 100, true, ObtemMunicipioPadrao(), ObtemMunicipioPadrao(),ObtemFornecedorPadrao(),
                ObtemTerminalPadrao(),100);

            processo.AdicionarItem(produto, 100, unidadeDeMedida);

            return processo;

        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComFornecedor()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao  = ObtemProcessoDeCotacaoDeFrete();
            processoDeCotacao.AdicionarFornecedor(ObtemFornecedorPadrao());
            processoDeCotacao.Abrir(ObtemUsuarioPadrao());
            return processoDeCotacao;
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada()
        {
            return ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada(ObtemMunicipioPadrao(), ObtemMunicipioPadrao());
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada(Municipio municipioOrigem, Municipio municipioDestino)
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = ObtemProcessoDeCotacaoDeFrete(municipioOrigem, municipioDestino);
            Fornecedor fornecedor = ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(ObtemUsuarioPadrao());
            processoDeCotacao.InformarCotacao(fornecedor.Codigo, 100, 10, "teste");
            return processoDeCotacao;
            
        }


        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada()
        {
            return ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(9);
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComDuasCotacoesSelecionadas()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = ObtemProcessoDeCotacaoDeFrete();

            Fornecedor fornecedor1 = ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor1);

            Fornecedor fornecedor2 = ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor2);

            processoDeCotacao.Abrir();

            CotacaoDeFrete cotacaoDeFrete1 = processoDeCotacao.InformarCotacao(fornecedor1.Codigo, 60, 9, "teste");
            cotacaoDeFrete1.Selecionar(10,0);

            CotacaoDeFrete cotacaoDeFrete2 = processoDeCotacao.InformarCotacao(fornecedor2.Codigo, 120, 20, "teste");
            cotacaoDeFrete2.Selecionar(5, 0);
            
            return processoDeCotacao;
        }


        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(decimal quantidadeAdquirida)
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada();
            var cotacaoItem = (CotacaoFreteItem)processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Itens.First();
            cotacaoItem.Selecionar(9);
            return processoDeCotacao;
        }


        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(Municipio municipioOrigem, Municipio municipioDestino)
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada(municipioOrigem, municipioDestino);
            var cotacaoItem = (CotacaoFreteItem)processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Itens.First();
            cotacaoItem.Selecionar(9);
            return processoDeCotacao;
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteFechado()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada();
            IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento = processoDeCotacao.FornecedoresSelecionados.Select(x => new CondicaoDoFechamentoNoSap
            {
                CodigoDoFornecedor = x.Fornecedor.Codigo,
                NumeroGeradoNoSap = "00001"
            });

            processoDeCotacao.FecharProcesso(condicoesDeFechamento);
            return processoDeCotacao;
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteFechado(Municipio municipioOrigem, Municipio municipioDestino)
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipioOrigem, municipioDestino);
            IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento = processoDeCotacao.FornecedoresSelecionados.Select(x => new CondicaoDoFechamentoNoSap
            {
                CodigoDoFornecedor = x.Fornecedor.Codigo,
                NumeroGeradoNoSap = "00001"
            });

            processoDeCotacao.FecharProcesso(condicoesDeFechamento);
            return processoDeCotacao;
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(Produto produto)
        {
            return ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto, ObtemMunicipioPadrao(),ObtemMunicipioPadrao());
        }

        public static ProcessoDeCotacaoDeFrete ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(Produto produto, Municipio municipioOrigem, Municipio municipioDestino)
        {
            var processo = new ProcessoDeCotacaoDeFrete(produto, 100, ObtemUnidadeDeMedidaPadrao(),
                "Requisitos do Processo de Cotação de Frete", "1000", DateTime.Today.AddDays(10),
                DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(2), ObtemItinerarioPadrao(),
                ObtemFornecedorPadrao(), 100, true, municipioOrigem, municipioDestino, ObtemFornecedorPadrao(),
                ObtemTerminalPadrao(),100);
            
			processo.AdicionarItem(produto, 100, ObtemUnidadeDeMedidaPadrao());
            return processo;

        }


        public static Quota ObtemQuotaDeCarregamento()
        {
            Terminal terminal = ObtemTerminalPadrao();
            return new Quota(ObterFarelo(),Enumeradores.FluxoDeCarga.Carregamento, ObtemTransportadoraPadrao(), terminal, DateTime.Today, 850);
        }

        public static Quota ObtemQuotaDeCarregamentoComDataEspecifica(DateTime data)
        {
            return ObtemQuotaDeCarregamentoComDataEspecifica(data, ObterFarelo());
        }

        public static Quota ObtemQuotaDeCarregamentoComDataEspecifica(DateTime data, MaterialDeCarga materialDeCarga)
        {
            Terminal terminal = ObtemTerminalPadrao();
            return new Quota(materialDeCarga, Enumeradores.FluxoDeCarga.Carregamento, ObtemTransportadoraPadrao(), terminal, data, 850);
        }



        public static Quota ObtemQuotaDeDescarregamento()
        {
            Terminal terminal = ObtemTerminalPadrao();
            return new Quota(ObterSoja(), Enumeradores.FluxoDeCarga.Descarregamento, ObtemTransportadoraPadrao(), terminal, DateTime.Today.AddDays(1), 850);
        }

        public static Quota ObtemQuotaDeDescarregamentoParaTerminalEspecifico(string codigoDoTerminal)
        {
            var terminal = new Terminal(codigoDoTerminal, "Terminal " + codigoDoTerminal, "Cidade" + codigoDoTerminal);
            return new Quota(EntidadesPersistidas.ObterSoja(), Enumeradores.FluxoDeCarga.Descarregamento, ObtemTransportadoraPadrao(), terminal, DateTime.Today.AddDays(1), 850);
        }


        public static NotaFiscalVm ObtemNotaFiscalVmPadrao()
        {
            return new NotaFiscalVm
                {
                    Numero = "1001",
                    Serie = "1",
                    DataDeEmissao = DateTime.Today.ToShortDateString(),
                    CnpjDoContratante = "111",
                    NomeDoContratante = "contratante",
                    CnpjDoEmitente = "222",
                    NomeDoEmitente = "emitente",
                    NumeroDoContrato = "500",
                    Peso = 50,
                    Valor = 1000
                };
        }

        public static NotaFiscalVm ObtemNotaFiscalVmComPesoEspecifico(decimal peso)
        {
            return new NotaFiscalVm
            {
                Numero = "1001",
                Serie = "1",
                DataDeEmissao = DateTime.Today.ToShortDateString(),
                CnpjDoContratante = "111",
                NomeDoContratante = "contratante",
                CnpjDoEmitente = "222",
                NomeDoEmitente = "emitente",
                NumeroDoContrato = "500",
                Peso = peso,
                Valor = 1000
            };
        }


        public static AgendamentoDeCarregamento ObtemAgendamentoDeCarregamentoComPesoEspecifico(Quota quota, decimal peso)
        {
            var factory = new AgendamentoDeCarregamentoFactory("Motorista", "Destino", peso);
            return (AgendamentoDeCarregamento) factory.Construir(quota, "IOQ5338");
        }


        public static AgendamentoDeDescarregamento ObtemAgendamentoDeDescarregamento(Quota quota)
        {
            var factory = new AgendamentoDeDescarregamentoFactory();
            factory.AdicionarNotaFiscal(ObtemNotaFiscalVmPadrao());
            return (AgendamentoDeDescarregamento)factory.Construir(quota, "IOQ5338");
        }

        public static PaginacaoVm ObtemPaginacaoDefault()
        {
            return new PaginacaoVm
                {
                    Page = 1,
                    PageSize = 10,
                    Take = 10
                };
        }

        public static OrdemDeTransporte ObtemOrdemDeTransporteComQuantidade(decimal quantidade)
        {
            ProcessoDeCotacaoDeFrete processoDeCotacaoDeFrete = ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(quantidade);
            IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento = processoDeCotacaoDeFrete.FornecedoresSelecionados.Select(x => new CondicaoDoFechamentoNoSap
            {
                CodigoDoFornecedor = x.Fornecedor.Codigo,
                NumeroGeradoNoSap = "00001"
            });

            return processoDeCotacaoDeFrete.FecharProcesso(condicoesDeFechamento).First();
        }

        public static OrdemDeTransporte ObtemOrdemDeTransporteComColeta()
        {
            OrdemDeTransporte ordemDeTransporte = ObtemOrdemDeTransporteComQuantidade(10);

            var fabricaDeColeta = new ColetaFactory();
            var coletaSalvarVm = new ColetaSalvarVm
            {
                DataDaColeta = DateTime.Now.Date.AddDays(-1).ToShortDateString(),
                DataDePrevisaoDeChegada = DateTime.Now.Date.ToShortDateString(),
                IdDaOrdemDeTransporte = ordemDeTransporte.Id,
                Motorista = "Mauro",
                Peso = 8,
                Placa = "ioq-5338",
                Realizado = "Não",
                NotasFiscais = new List<NotaFiscalDeColetaVm>
                {
                    new NotaFiscalDeColetaVm
                    {
                        DataDeEmissao = DateTime.Now.Date.ToShortDateString(),
                        Peso = 8,
                        Numero = "123434",
                        Serie = "1",
                        Valor = 5435M
                    }
                }

            };

            Coleta coleta = fabricaDeColeta.Construir(coletaSalvarVm, ordemDeTransporte.PrecoUnitario);


            ordemDeTransporte.AdicionarColeta(coleta);      
      
            return ordemDeTransporte;

        }

        public static MaterialDeCarga ObterSoja()
        {
            return new MaterialDeCarga("Soja");
        }

        public static MaterialDeCarga ObterFarelo()
        {
            return new MaterialDeCarga("Farelo");
        }

        public static ConhecimentoDeTransporte ObterConhecimentoDeTransporte()
        {
            var conhecimentoDeTransporte = new ConhecimentoDeTransporte("42131025174182000157550010000000020108042108", "518505925", "2q34342423",
                DateTime.Today, "1", "100", "24Q424", 1000,100000);

            conhecimentoDeTransporte.AdicionarNotaFiscal("42131084684182000157550010000000020108042108","12","1");
            conhecimentoDeTransporte.AdicionarNotaFiscal("42131084682582000157550010000000020108042108","13","1");

            return conhecimentoDeTransporte;

        }
    }

    internal class RequisicaoDeCompraTeste:RequisicaoDeCompra
    {
        internal RequisicaoDeCompraTeste(int id, Usuario criador, string requisitante, Fornecedor fornecedorPretendido,
                                         DateTime dataDeRemessa, DateTime dataDeLiberacao, DateTime dataDeSolicitacao,
                                         string centro,
                                         UnidadeDeMedida unidadeMedida, decimal quantidade, Produto material,
                                         string descricao, string numeroItem,
                                         string numero, string codigoGrupoDeCompra, bool mrp)
            : base(criador, requisitante, fornecedorPretendido, dataDeRemessa,
                   dataDeLiberacao, dataDeSolicitacao, centro, unidadeMedida, quantidade, material, descricao,
                   numeroItem, numero, codigoGrupoDeCompra, mrp)
        {
            Id = id;
        }
        
    }

        
}
