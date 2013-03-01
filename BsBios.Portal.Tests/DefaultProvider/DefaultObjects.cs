﻿using System;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Tests.DefaultProvider
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

        private static string GeraCodigo(int contador, int tamanho)
        {
            string contadorToString = Convert.ToString(contador);
            return new string('0', tamanho - contadorToString.Length) + contador;

        }

        public static string ObtemSufixoCodigoFornecedor(string codigoFornecedor)
        {
            return codigoFornecedor.Substring(9);
        }

        public static RequisicaoDeCompra ObtemRequisicaoDeCompraPadrao()
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

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, "requisitante", fornecedorPretendido,
                dataDeRemessa, dataDeLiberacao, dataDeSolicitacao, "C001", "UNT", 1000,
                material, "Requisição de Compra enviada pelo SAP", numeroItem, numeroRequisicao);
            
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
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra, requisicaoDeCompra.Material, requisicaoDeCompra.Quantidade);
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
            var codigo = GeraCodigo(_contadorFornecedores, 10);
            var fornecedor = new Fornecedor(codigo, "FORNECEDOR " + codigo, 
                "fornecedor" + codigo + "@empresa.com.br");
            return fornecedor;
        }

        public static Usuario ObtemUsuarioPadrao()
        {
            _contadorUsuarios++;
            var codigo = GeraCodigo(_contadorUsuarios, 12);

            var usuario = new Usuario("Usuário " + codigo, codigo, 
                "usuario" + codigo + "@empresa.com.br", Enumeradores.Perfil.Comprador);
            return usuario;
        }

        public static Produto ObtemProdutoPadrao()
        {
            _contadorProdutos++;
            var codigo = GeraCodigo(_contadorProdutos, 18);
            var produto = new Produto(codigo, "PRODUTO " + codigo, "01");
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
        
    }
}
