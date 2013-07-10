using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ComunicacaoFechamentoProcessoCotacaoFrete : IProcessoDeCotacaoComunicacaoSap
    {
        private readonly IComunicacaoSap<ListaProcessoDeCotacaoDeFreteFechamento> _comunicacaoSap;

        public ComunicacaoFechamentoProcessoCotacaoFrete(IComunicacaoSap<ListaProcessoDeCotacaoDeFreteFechamento> comunicacaoSap)
        {
            _comunicacaoSap = comunicacaoSap;
        }

        //public ComunicacaoFechamentoProcessoCotacaoFrete(CredencialSap credencialSap)
        //{
        //    _comunicacaoSap = new ComunicacaoSap<ListaProcessoDeCotacaoDeFreteFechamento>(credencialSap);
        //}

        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo)
        {
            ProcessoDeCotacaoItem item = processo.Itens.First();
            if (item.Produto.NaoEstocavel)
            {
                //Cotações de frete para empresas do grupo que não utilizam o SAP deverão ser realizadas com material NLAG 
                //(Material não estocável). Para este tipo de material a cotação não deverá ser enviada para o SAP;
                return new ApiResponseMessage
                    {
                        Retorno = new Retorno
                            {
                                Codigo = "200",
                                Texto = "S"
                            }
                    };
            }

            var mensagemParaEnviar = new ListaProcessoDeCotacaoDeFreteFechamento();

            var processoAuxiliar = (ProcessoDeCotacaoDeFrete) processo.CastEntity();

            foreach (var fornecedorParticipante in processoAuxiliar.FornecedoresParticipantes)
            {
                if (fornecedorParticipante.Cotacao != null && fornecedorParticipante.Cotacao.Itens.First().Selecionada)
                {
                    CotacaoItem itemDaCotacao = fornecedorParticipante.Cotacao.Itens.First();
                    mensagemParaEnviar.Add(new ProcessoDeCotacaoDeFreteFechamentoComunicacaoSapVm
                        {
                            CodigoTransportadora = fornecedorParticipante.Fornecedor.Codigo,
                            //CodigoMaterial = processoAuxiliar.Produto.Codigo,
                            CodigoMaterial =  item.Produto.Codigo,
                            //CodigoUnidadeMedida = processoAuxiliar.UnidadeDeMedida.CodigoInterno,
                            CodigoUnidadeMedida = item.UnidadeDeMedida.CodigoInterno,
                            CodigoItinerario = processoAuxiliar.Itinerario.Codigo,
                            DataDeValidadeInicial = processoAuxiliar.DataDeValidadeInicial.ToString("yyyyMMdd"),
                            DataDeValidaFinal = processoAuxiliar.DataDeValidadeFinal.ToString("yyyyMMdd"),
                            NumeroDoContrato = processoAuxiliar.NumeroDoContrato ?? "",
                            Valor = itemDaCotacao.ValorComImpostos
                        });
                }
            }

            ApiResponseMessage apiResponseMessage =
                _comunicacaoSap.EnviarMensagem(
                    "/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_cotacaoFreteFechamento_portal&qos=be"
                    , mensagemParaEnviar);
            if (apiResponseMessage.Retorno.Codigo == "E")
            {
                throw new ComunicacaoSapException("json","Ocorreu um erro ao comunicar o fechamento do Processo de Cotação de Frete para o SAP. Detalhes: " + apiResponseMessage.Retorno.Texto);
            }
            return apiResponseMessage;

        }
    }
}