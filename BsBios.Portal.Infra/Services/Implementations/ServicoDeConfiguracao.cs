using System;
using System.Configuration;
using System.Web.Configuration;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ServicoDeConfiguracao : IServicoDeConfiguracao
    {
        public Configuracao ObterConfiguracao()
        {
            var section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            var configuracao = new Configuracao() ;
            if (section != null)
            {
                //o tamanho no arquivo de configuração está em kilobytes (KB)
                configuracao.TamanhoMaximoDaRequisicaoEmBytes = section.MaxRequestLength * 1024;
                configuracao.TamanhoMaximoDaRequisicaoEmMegaBytes = Math.Round((decimal) section.MaxRequestLength / 1024 , 1);
            }

            return configuracao;
        }
    }
}