using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using FluentNHibernate.Utils;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeMensagemDeEmail : IGeradorDeMensagemDeEmail
    {

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

        public MensagemDeEmail AberturaDoProcessoDeCotacaoDeFrete(ProcessoDeCotacao processoDeCotacao)
        {
            var processoDeCotacaoDeFrete = (ProcessoDeCotacaoDeFrete) processoDeCotacao;
            string mensagem = "Prezado Fornecedor. " + Environment.NewLine +
                              "A BSBIOS convida a participar do nosso processo de cotação para o Produto/Serviço " +
                              "conforme informações descritas abaixo. " + Environment.NewLine +
                              "Caso tenha interesse em participar favor acessar o Portal de Cotações" +
                              Environment.NewLine + Environment.NewLine +
                              "Material: " + processoDeCotacaoDeFrete.Produto.Descricao + Environment.NewLine +
                              "Quantidade: " + processoDeCotacaoDeFrete.Quantidade + Environment.NewLine +
                              "Unidade de Medida: " + processoDeCotacaoDeFrete.UnidadeDeMedida.Descricao +
                              Environment.NewLine +
                              "Data Limite de Retorno: " + (processoDeCotacaoDeFrete.DataLimiteDeRetorno.HasValue ? processoDeCotacaoDeFrete.DataLimiteDeRetorno.Value.ToShortDateString() : "") + Environment.NewLine +
                              "Requisitos: " + processoDeCotacaoDeFrete.Requisitos + Environment.NewLine +
                              "Itinerário: " + processoDeCotacaoDeFrete.Itinerario.Descricao + Environment.NewLine +
                              "Município de Origem: " + (processoDeCotacaoDeFrete.MunicipioDeOrigem != null ? processoDeCotacaoDeFrete.MunicipioDeOrigem.ToString(): "") + Environment.NewLine +
                              "Município de Destino: " + (processoDeCotacaoDeFrete.MunicipioDeDestino != null ? processoDeCotacaoDeFrete.MunicipioDeDestino.ToString(): "") + Environment.NewLine +
                              "Cadência: " + processoDeCotacaoDeFrete.Cadencia  + Environment.NewLine +
                              "Classificação: " + (processoDeCotacaoDeFrete.Classificacao ? "Sim" : "Não")  + Environment.NewLine +
                              "Fornecedor da Mercadoria: " + (processoDeCotacaoDeFrete.FornecedorDaMercadoria != null ? processoDeCotacaoDeFrete.FornecedorDaMercadoria.Nome: "") + Environment.NewLine + 
                              "Depósito: " + (processoDeCotacaoDeFrete.Deposito != null ? processoDeCotacaoDeFrete.Deposito.Nome : "") + Environment.NewLine;
                              
            return new MensagemDeEmail("Cotação de Frete", mensagem);
        }

        public MensagemDeEmail AberturaDoProcessoDeCotacaoDeMaterial(ProcessoDeCotacao processoDeCotacao)
        {
            string mensagem = "Prezado Fornecedor. " + Environment.NewLine +
                              "A BSBIOS convida a participar do nosso processo de cotação para o Produto/Serviço " +
                              "conforme informações descritas abaixo. " + Environment.NewLine +
                              "Caso tenha interesse em participar favor acessar o Portal de Cotações" + Environment.NewLine + Environment.NewLine +
                              "Material: " + processoDeCotacao.Produto.Descricao + Environment.NewLine +
                              "Quantidade: " + processoDeCotacao.Quantidade + Environment.NewLine +
                              "Unidade de Medida: " + processoDeCotacao.UnidadeDeMedida.Descricao + Environment.NewLine +
                              "Data Limite de Retorno: " + (processoDeCotacao.DataLimiteDeRetorno.HasValue ? processoDeCotacao.DataLimiteDeRetorno.Value.ToShortDateString() : "") + Environment.NewLine +
                              "Requisitos: " + processoDeCotacao.Requisitos + Environment.NewLine;

            return new MensagemDeEmail("Cotação de Material", mensagem);
        }

        public MensagemDeEmail FornecedoresSelecionadosNoProcessoDeCotacao(ProcessoDeCotacao processoDeCotacao, Cotacao cotacao)
        {
            string mensagem = "Prezado Fornecedor." + Environment.NewLine +
                              "Estamos confirmando o fechamento da negociação referente ao Processo de Cotação nº " + cotacao.Id + "." + Environment.NewLine +
                              "Segue nosso Pedido de Compras." + Environment.NewLine + Environment.NewLine +  
                              "Material: "  + processoDeCotacao.Produto.Descricao + Environment.NewLine +
                              "Quantidade: " + cotacao.QuantidadeAdquirida + Environment.NewLine +
                              "Unidade de Medida: " + processoDeCotacao.UnidadeDeMedida.Descricao + Environment.NewLine +
                              "Para maiores esclarecimentos, favor entrar em contato com o comprador.";

            return new MensagemDeEmail("Fechamento do Processo de Cotacão", mensagem);
        }

        public MensagemDeEmail FornecedoresNaoSelecionadosNoProcessoDeCotacao(Cotacao cotacao)
        {
            string mensagem = "Prezado Fornecedor."  + Environment.NewLine + 
                "Agradecemos sua partipação no nosso processo de cotação " + cotacao.Id  +
                " e informamos que optamos por fechar a negociação com outro fornecedor." + Environment.NewLine + 
                "Ficaremos gratos caso tenha interesse em participar de nossas futuras cotações.";

            return new MensagemDeEmail("Fechamento do Processo de Cotacão",mensagem);
        }

        public MensagemDeEmail AutorizacaoDeTransporte(ProcessoDeCotacaoDeFrete processoDeCotacao)
        {
            var descricaoDaUnidadeDeMedida = processoDeCotacao.UnidadeDeMedida.Descricao;
            var mensagem = "Prezado Fornecedor." + Environment.NewLine +
                           "Informamos que as seguintes transportadoras foram autorizadas a coletar " +
                           processoDeCotacao.Quantidade + " " + descricaoDaUnidadeDeMedida +
                           " de " + processoDeCotacao.Produto.Descricao + " entre " +
                           processoDeCotacao.DataDeValidadeInicial.ToShortDateString() + " e " +
                           processoDeCotacao.DataDeValidadeFinal.ToShortDateString() + ":" + Environment.NewLine +
                           Environment.NewLine;

            foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresSelecionados)
            {
                mensagem += string.Format("Transportada: {0} - Quantidade: {1} - Unidade de Medida: {2}",
                    fornecedorParticipante.Fornecedor.Nome, fornecedorParticipante.Cotacao.QuantidadeAdquirida, descricaoDaUnidadeDeMedida)
                    + Environment.NewLine;
            }

            mensagem += Environment.NewLine + _mensagemDeRodape;

            return new MensagemDeEmail("Autorização de transporte de mercadoria", mensagem);

        }
    }
}