using System;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeMensagemDeEmail : IGeradorDeMensagemDeEmail
    {
        private const string SeparadorDeItems = "----------------------------";
        public MensagemDeEmail CriacaoAutomaticaDeSenha(Usuario usuario, string novaSenha)
        {
            string mensagem = "Prezado(a) " + usuario.Nome + Environment.NewLine + Environment.NewLine +
            "Conforme foi solicitado através do Portal de Cotações da BSBIOS, segue abaixo a sua nova senha de acesso ao site. " + 
            "Esta senha foi gerada automaticamente no momento da sua solicitação. "+
            "Recomenda-se que acesse o site http://bsnet.bsbios.com/ e altere a senha para uma de sua preferência." + Environment.NewLine + Environment.NewLine +
            "Dados de Acesso:" + Environment.NewLine + Environment.NewLine + 
            "Login: " + usuario.Login + Environment.NewLine +
            "Nova Senha: " + novaSenha + Environment.NewLine +
            "Atenciosamente," + Environment.NewLine +
            "BSBIOS" + Environment.NewLine + Environment.NewLine +
            "Esta é uma mensagem gerada automaticamente, portanto, não deve ser respondida." + Environment.NewLine +
            "© BSBIOS. Todos os direitos reservados. Termos e Condições e Política de Privacidade." + Environment.NewLine;

            return new MensagemDeEmail("Geração automática de senha",mensagem);
        }

        public MensagemDeEmail AberturaDoProcessoDeCotacaoDeFrete(ProcessoDeCotacao processoDeCotacao)
        {
            ProcessoDeCotacaoItem item = processoDeCotacao.Itens.First();
            var processoDeCotacaoDeFrete = (ProcessoDeCotacaoDeFrete) processoDeCotacao;
            string mensagem = "Prezado Fornecedor. " + Environment.NewLine +
                              "A BSBIOS convida a participar do nosso processo de cotação para o Produto/Serviço " +
                              "conforme informações descritas abaixo. " + Environment.NewLine +
                              "Caso tenha interesse em participar favor acessar o Portal de Cotações" + Environment.NewLine + Environment.NewLine +
                              //"Material: " + processoDeCotacaoDeFrete.Produto.Descricao + Environment.NewLine +
                              "Material: " + item.Produto.Descricao + Environment.NewLine +
                              //"Quantidade: " + processoDeCotacaoDeFrete.Quantidade + Environment.NewLine +
                              "Quantidade: " + item.Quantidade + Environment.NewLine +
                              "Itinerário: " + processoDeCotacaoDeFrete.Itinerario.Descricao + Environment.NewLine +
                              //"Unidade de Medida: " + processoDeCotacaoDeFrete.UnidadeDeMedida.Descricao + Environment.NewLine +
                              "Unidade de Medida: " + item.UnidadeDeMedida.Descricao + Environment.NewLine +
                              "Data Limite de Retorno: " + (processoDeCotacaoDeFrete.DataLimiteDeRetorno.HasValue ? processoDeCotacaoDeFrete.DataLimiteDeRetorno.Value.ToShortDateString() : "") + Environment.NewLine +
                              "Requisitos: " + processoDeCotacaoDeFrete.Requisitos + Environment.NewLine;

            return new MensagemDeEmail("Cotação de Frete", mensagem);
        }

        public MensagemDeEmail AberturaDoProcessoDeCotacaoDeMaterial(ProcessoDeCotacao processoDeCotacao)
        {
            string mensagem = "Prezado Fornecedor. " + Environment.NewLine +
                              "A BSBIOS convida a participar do nosso processo de cotação para o Produto/Serviço " +
                              "conforme informações descritas abaixo. " + Environment.NewLine +
                              "Caso tenha interesse em participar favor acessar o Portal de Cotações" +
                              Environment.NewLine + Environment.NewLine;

            for (int i = 0; i < processoDeCotacao.Itens.Count; i++)
            {
                var item = processoDeCotacao.Itens[i];
                mensagem +=
                  "Item " + Convert.ToString(i) + Environment.NewLine +
                  "Material: " + item.Produto.Descricao + Environment.NewLine +
                  "Quantidade: " + item.Quantidade + Environment.NewLine +
                  "Unidade de Medida: " + item.UnidadeDeMedida.Descricao + Environment.NewLine + 
                  SeparadorDeItems  + Environment.NewLine;
        
            }

            mensagem +=
                              "Data Limite de Retorno: " + (processoDeCotacao.DataLimiteDeRetorno.HasValue ? processoDeCotacao.DataLimiteDeRetorno.Value.ToShortDateString() : "") + Environment.NewLine +
                              "Requisitos: " + processoDeCotacao.Requisitos + Environment.NewLine;

            return new MensagemDeEmail("Cotação de Material", mensagem);
        }

        public MensagemDeEmail FornecedoresSelecionadosNoProcessoDeCotacao(ProcessoDeCotacao processoDeCotacao, Cotacao cotacao)
        {
            string mensagem = "Prezado Fornecedor." + Environment.NewLine +
                              "Estamos confirmando o fechamento da negociação referente " + cotacao.Id + "." + Environment.NewLine +
                              "Segue nosso Pedido de Compras." + Environment.NewLine + Environment.NewLine;

            for (int i = 0; i < processoDeCotacao.Itens.Count; i++)
            {
                var item = processoDeCotacao.Itens[i];
                mensagem +=
                    "Item " + Convert.ToString(i) + Environment.NewLine +
                    "Material: " + item.Produto.Descricao + Environment.NewLine +
                    "Quantidade: " + cotacao.QuantidadeAdquirida + Environment.NewLine +
                    "Unidade de Medida: " + item.UnidadeDeMedida.Descricao + Environment.NewLine + 
                    SeparadorDeItems + Environment.NewLine;
            }
            mensagem += " Para maiores esclarecimentos, favor entrar em contato através com o comprador.";

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
    }
}