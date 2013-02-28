using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Tests.DefaultProvider
{
    public static class DefaultObjects
    {
        private static int _contadorFornecedores = 0;
        private static int _contadorProdutos = 0;
        private static int _contadorCondicoesDePagamento = 0;
        private static int _contadorIvas = 0;
        private static int _contadorIncoterms = 0;
        private static int _contadorUsuarios = 0;

        public static RequisicaoDeCompra ObtemRequisicaoDeCompraPadrao()
        {
            var usuarioCriador = ObtemUsuarioPadrao();
            var fornecedorPretendido = ObtemFornecedorPadrao();
            var material = ObtemProdutoPadrao();

            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, "requisitante", fornecedorPretendido,
                dataDeRemessa, dataDeLiberacao, dataDeSolicitacao, "C001", "UNT", 1000,
                material, "Requisição de Compra enviada pelo SAP", "00001", "REQ0001");
            
            return requisicaoDeCompra;
        }

        public static RequisicaoDeCompra ObtemRequisicaoDeCompraSemRequisitanteEFornecedor()
        {
            var usuarioCriador = ObtemUsuarioPadrao();
            var material = ObtemProdutoPadrao();

            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, null, null,
                dataDeRemessa, dataDeLiberacao, dataDeSolicitacao, "C001", "UNT", 1000,
                material, "Requisição de Compra enviada pelo SAP", "00001", "REQ0001");

            return requisicaoDeCompra;
        }


        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialPadrao()
        {
            var requisicaoDeCompra = ObtemRequisicaoDeCompraPadrao();
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra, requisicaoDeCompra.Material, 100);
            return processoDeCotacao;
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoAbertoPadrao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = ObtemProcessoDeCotacaoDeMaterialPadrao();
            Fornecedor fornecedor = ObtemFornecedorPadrao();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir();
            return processoDeCotacao;
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialFechado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = ObtemProcessoDeCotacaoAbertoPadrao();
            var codigoFornecedor = processoDeCotacao.Fornecedores.First().Codigo;
            processoDeCotacao.AtualizarCotacao(codigoFornecedor,125, ObtemIncotermPadrao(),"Descrição do Incotem");
            processoDeCotacao.SelecionarCotacao(codigoFornecedor, 100, ObtemIvaPadrao(), ObtemCondicaoDePagamentoPadrao());
            processoDeCotacao.Fechar();
            return processoDeCotacao;
        }

        public static Fornecedor ObtemFornecedorPadrao()
        {
            _contadorFornecedores++;
            var fornecedor = new Fornecedor("FORNEC000" + _contadorFornecedores, "FORNECEDOR 000" + _contadorFornecedores, 
                "fornecedor000" + _contadorFornecedores + "@empresa.com.br");
            return fornecedor;
        }

        public static Usuario ObtemUsuarioPadrao()
        {
            _contadorUsuarios++;
            var usuario = new Usuario("Usuario 000" + _contadorUsuarios, "usuario000" + _contadorUsuarios, 
                "usuario000" + _contadorUsuarios + "@empresa.com.br", Enumeradores.Perfil.Comprador);
            return usuario;
        }

        public static Produto ObtemProdutoPadrao()
        {
            _contadorProdutos++;
            var produto = new Produto("PROD000" + _contadorProdutos, "PRODUTO 000" + _contadorProdutos, "01");
            return produto;
        }

        public static Incoterm ObtemIncotermPadrao()
        {
            _contadorIncoterms ++;
            return new Incoterm("0" + _contadorIncoterms, "INCOTERM 0" + _contadorIncoterms);
        }

        public static Iva ObtemIvaPadrao()
        {
            _contadorIvas++;
            return new Iva(Convert.ToString(_contadorIvas), "IVA " + _contadorIvas);
        }

        public static CondicaoDePagamento ObtemCondicaoDePagamentoPadrao()
        {
            _contadorCondicoesDePagamento++;
            return new CondicaoDePagamento("C"+ _contadorCondicoesDePagamento , "CONDIÇÃO 00" + _contadorCondicoesDePagamento);
        }
        
    }
}
