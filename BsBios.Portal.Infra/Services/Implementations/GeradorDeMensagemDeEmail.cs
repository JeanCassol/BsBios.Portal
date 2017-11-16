using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Email;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeMensagemDeEmail : IGeradorDeMensagemDeEmail
    {
        private const string SeparadorDeItems = "----------------------------";
        private readonly string _mensagemDeRodape = "Atenciosamente," + Environment.NewLine +
                                                    "BSBIOS" + Environment.NewLine + Environment.NewLine +
                                                    "Esta é uma mensagem gerada automaticamente, portanto, não deve ser respondida." + Environment.NewLine +
                                                    "© BSBIOS. Todos os direitos reservados. Termos e Condições e Política de Privacidade." + Environment.NewLine;

        public MensagemDeEmail CriacaoAutomaticaDeSenha(Usuario usuario, string novaSenha)
        {
            string mensagem = "Prezado(a) " + usuario.Nome + Environment.NewLine + Environment.NewLine +
                              "Conforme foi solicitado através do Portal de Cotações da BSBIOS, segue abaixo a sua nova senha de acesso ao site. " +
                              "Esta senha foi gerada automaticamente no momento da sua solicitação. " +
                              "Recomenda-se que acesse o site http://bsnet.bsbios.com/ e altere a senha para uma de sua preferência." +
                              Environment.NewLine + Environment.NewLine +
                              "Dados de Acesso:" + Environment.NewLine + Environment.NewLine +
                              "Login: " + usuario.Login + Environment.NewLine +
                              "Nova Senha: " + novaSenha + Environment.NewLine +
                              _mensagemDeRodape;

            return new MensagemDeEmail("Geração automática de senha",mensagem);
        }

        public MensagemDeEmail AberturaDoProcessoDeCotacaoDeFrete(ProcessoDeCotacaoDeFrete processoDeCotacao)
        {
            var processoDeCotacaoItem = (ProcessoDeCotacaoDeFreteItem) processoDeCotacao.Itens.First();
            string mensagem = "Prezado Fornecedor. " + Environment.NewLine +
                              "A BSBIOS convida a participar do nosso processo de cotação para o Produto/Serviço " +
                              "conforme informações descritas abaixo. " + Environment.NewLine +
                              "Caso tenha interesse em participar favor acessar o Portal de Cotações" +
                              Environment.NewLine + Environment.NewLine +
                              "Material: " + processoDeCotacaoItem.Produto.Descricao + Environment.NewLine +
                              "Quantidade: " + processoDeCotacaoItem.Quantidade + Environment.NewLine +
                              "Unidade de Medida: " + processoDeCotacaoItem.UnidadeDeMedida.Descricao + Environment.NewLine +
                              "Data Limite de Retorno: " + (processoDeCotacao.DataLimiteDeRetorno?.ToShortDateString() ?? "") + Environment.NewLine +
                              "Requisitos: " + processoDeCotacao.Requisitos + Environment.NewLine +
                              "Itinerário: " + processoDeCotacao.Itinerario.Descricao + Environment.NewLine +
                              "Município de Origem: " + (processoDeCotacao.MunicipioDeOrigem?.ToString() ?? "") + Environment.NewLine +
                              "Município de Destino: " + (processoDeCotacao.MunicipioDeDestino?.ToString() ?? "") + Environment.NewLine +
                              "Cadência: " + processoDeCotacaoItem.Cadencia  + Environment.NewLine +
                              "Classificação: " + (processoDeCotacao.Classificacao ? "Sim" : "Não")  + Environment.NewLine +
                              "Fornecedor da Mercadoria: " + (processoDeCotacao.FornecedorDaMercadoria != null ? processoDeCotacao.FornecedorDaMercadoria.Nome: "") + Environment.NewLine + 
                              "Depósito: " + (processoDeCotacao.Deposito != null ? processoDeCotacao.Deposito.Nome : "") + Environment.NewLine;
                              
            return new MensagemDeEmail("Cotação de Frete", mensagem);
        }

        public MensagemDeEmail AberturaDoProcessoDeCotacaoDeMaterial(ProcessoDeCotacao processoDeCotacao)
        {
            string mensagem = "Prezado Fornecedor. " + Environment.NewLine +
                              "A BSBIOS convida a participar do nosso processo de cotação para o Produto/Serviço " +
                              "conforme informações descritas abaixo. " + Environment.NewLine +
                              "Caso tenha interesse em participar favor acessar o Portal de Cotações (http://bsnet.bsbios.com/)." +
                              Environment.NewLine + Environment.NewLine;

            for (int i = 0; i < processoDeCotacao.Itens.Count; i++)
            {
                var item = processoDeCotacao.Itens[i];
                mensagem +=
                  "Item " + Convert.ToString(i + 1) + Environment.NewLine +
                  "Material: " + item.Produto.Descricao + Environment.NewLine +
                  "Quantidade: " + item.Quantidade + Environment.NewLine +
                  "Unidade de Medida: " + item.UnidadeDeMedida.Descricao + Environment.NewLine + 
                  SeparadorDeItems  + Environment.NewLine;
            }

            mensagem +=
                "Data Limite de Retorno: " + (processoDeCotacao.DataLimiteDeRetorno?.ToShortDateString() ?? "") + Environment.NewLine +
                "Requisitos: " + processoDeCotacao.Requisitos + Environment.NewLine +
                SeparadorDeItems + Environment.NewLine +
                "Comprador" + Environment.NewLine +
                "Nome: " + processoDeCotacao.Comprador.Nome + Environment.NewLine +
                "E-mail: " + processoDeCotacao.Comprador.Email;

            return new MensagemDeEmail("Cotação de Material", mensagem);
        }

        public MensagemDeEmail FornecedoresSelecionadosNoProcessoDeCotacao(ProcessoDeCotacao processoDeCotacao, Cotacao cotacao)
        {
            string mensagem = "Prezado Fornecedor." + Environment.NewLine +
                              "Estamos confirmando o fechamento da negociação referente ao Processo de Cotação " + processoDeCotacao.Id + "." + Environment.NewLine +
                              "Segue nosso Pedido de Compras." + Environment.NewLine + Environment.NewLine;

            IList<CotacaoItem> itensDaCotacao = cotacao.Itens.Where(item => item.Selecionada).ToList();

            for (int i = 0; i < itensDaCotacao.Count; i++)
            {
                var item = itensDaCotacao[i];
                mensagem +=
                    "Item " + Convert.ToString(i + 1) + Environment.NewLine +
                    "Material: " + item.ProcessoDeCotacaoItem.Produto.Descricao + Environment.NewLine +
                    "Quantidade: " + item.QuantidadeAdquirida + Environment.NewLine +
                    "Unidade de Medida: " + item.ProcessoDeCotacaoItem.UnidadeDeMedida.Descricao + Environment.NewLine + 
                    SeparadorDeItems + Environment.NewLine;
            }
            mensagem += " Para maiores esclarecimentos, favor entrar em contato através com o comprador.";

            return new MensagemDeEmail("Fechamento do Processo de Cotacão", mensagem);
        }


        public MensagemDeEmail FornecedoresSelecionadosNoProcessoDeCotacaoDeFrete(ProcessoDeCotacaoDeFrete processoDeCotacao, Cotacao cotacao)
        {
            var processoCotacaoItem = (ProcessoDeCotacaoDeFreteItem) processoDeCotacao.Itens.First();
            var cotacaoItem = cotacao.Itens.First();
            string mensagem = "Prezado Fornecedor." + Environment.NewLine +
                              "Estamos confirmando o fechamento da negociação referente ao Processo de Cotação nº " + processoDeCotacao.Id + "." + Environment.NewLine +
                              "Segue nosso Pedido de Compras." + Environment.NewLine + Environment.NewLine +
                              "Material: " + processoCotacaoItem.Produto.Descricao + Environment.NewLine +
                              "Quantidade: " + cotacaoItem.QuantidadeAdquirida + Environment.NewLine +
                              "Unidade de Medida: " + processoCotacaoItem.UnidadeDeMedida.Descricao + Environment.NewLine +
                              "Requisitos: " + processoDeCotacao.Requisitos + Environment.NewLine +
                              "Itinerário: " + processoDeCotacao.Itinerario.Descricao + Environment.NewLine +
                              "Município de Origem: " + (processoDeCotacao.MunicipioDeOrigem?.ToString() ?? "") + Environment.NewLine +
                              "Município de Destino: " + (processoDeCotacao.MunicipioDeDestino?.ToString() ?? "") + Environment.NewLine +
                              "Cadência: " + processoCotacaoItem.Cadencia + Environment.NewLine +
                              "Classificação: " + (processoDeCotacao.Classificacao ? "Sim" : "Não") + Environment.NewLine +
                              "Fornecedor da Mercadoria: " + (processoDeCotacao.FornecedorDaMercadoria != null ? processoDeCotacao.FornecedorDaMercadoria.Nome : "") + Environment.NewLine +
                              "Depósito: " + (processoDeCotacao.Deposito != null ? processoDeCotacao.Deposito.Nome : "") + Environment.NewLine + Environment.NewLine +
                              "Para maiores esclarecimentos, favor entrar em contato com o comprador.";

            return new MensagemDeEmail("Fechamento do Processo de Cotacão", mensagem);
        }


        public MensagemDeEmail FornecedoresNaoSelecionadosNoProcessoDeCotacao(ProcessoDeCotacao processoDeCotacao)
        {
            string mensagem = "Prezado Fornecedor."  + Environment.NewLine + 
                "Agradecemos sua partipação no nosso processo de cotação " + processoDeCotacao.Id  +
                " e informamos que optamos por fechar a negociação com outro fornecedor." + Environment.NewLine + 
                "Ficaremos gratos caso tenha interesse em participar de nossas futuras cotações.";

            return new MensagemDeEmail("Fechamento do Processo de Cotacão",mensagem);
        }

        public MensagemDeEmail AutorizacaoDeTransporte(ProcessoDeCotacaoDeFrete processoDeCotacao)
        {
            var processoDeCotacaoItem = processoDeCotacao.Itens.First();
            var descricaoDaUnidadeDeMedida = processoDeCotacaoItem.UnidadeDeMedida.Descricao;
            var mensagem = "Prezado Fornecedor." + Environment.NewLine +
                           "Informamos que as seguintes transportadoras foram autorizadas a coletar " +
                           processoDeCotacaoItem.Quantidade + " " + descricaoDaUnidadeDeMedida +
                           " de " + processoDeCotacaoItem.Produto.Descricao + " entre " +
                           processoDeCotacao.DataDeValidadeInicial.ToShortDateString() + " e " +
                           processoDeCotacao.DataDeValidadeFinal.ToShortDateString() + ":" + Environment.NewLine +
                           Environment.NewLine;

            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresSelecionados)
            {
                
                var cotacaoDeFrete = (CotacaoDeFrete) fornecedorParticipante.Cotacao.CastEntity();
                var cotacaoItem = (CotacaoFreteItem) cotacaoDeFrete.Itens.First();
                mensagem += $"Transportada: {fornecedorParticipante.Fornecedor.Nome} - Quantidade: {cotacaoItem.QuantidadeAdquirida} - Unidade de Medida: {descricaoDaUnidadeDeMedida} - Cadência: {cotacaoItem.Cadencia} - Classificação: {(processoDeCotacao.Classificacao ? "Sim" : "Não")} - Nº do Contrato: {processoDeCotacao.NumeroDoContrato}"
                    + Environment.NewLine;
            }

            mensagem += Environment.NewLine + _mensagemDeRodape;

            return new MensagemDeEmail("Autorização de transporte de mercadoria", mensagem);

        }
    }
}