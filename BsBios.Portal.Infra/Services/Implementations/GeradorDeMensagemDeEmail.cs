using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeMensagemDeEmail : IGeradorDeMensagemDeEmail
    {
        public MensagemDeEmail CriacaoAutomaticaDeSenha(Usuario usuario, string novaSenha)
        {
            string mensagem = "Prezado(a) " + usuario.Nome + Environment.NewLine + Environment.NewLine +
            "Conforme foi solicitado através do Portal de Cotações da BSBIOS, segue abaixo a sua nova senha de acesso ao site. " + 
            "Esta senha foi gerada automaticamente no momento da sua solicitação. "+
            "Recomenda-se que acesse o site e altere a senha para uma de sua preferência."  + Environment.NewLine + Environment.NewLine +
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
            string mensagem = "Fornecedor, você está convidado a participar do Processo de Cotação de Fretes." +
                              Environment.NewLine +
                              "Material: " + processoDeCotacao.Produto.Descricao + Environment.NewLine +
                              "Quantidade: " + processoDeCotacao.Quantidade + Environment.NewLine +
                              "Unidade de Medida: " + processoDeCotacao.UnidadeDeMedida.Descricao + Environment.NewLine +
                              "Data Limite de Retorno:  " + processoDeCotacao.DataLimiteDeRetorno.ToString() + Environment.NewLine +
                              "Para informar a sua cotação acesse o Portal da BS BIOS até a data limite de retorno.";

            return new MensagemDeEmail("Cotação de Frete", mensagem);
        }
    }
}