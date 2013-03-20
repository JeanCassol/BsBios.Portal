using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ComunicacaoFechamentoProcessoCotacaoFrete : IComunicacaoSap
    {
        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo)
        {
            var clientHandler = new HttpClientHandler {Credentials = new NetworkCredential("fusion_lucas", "fusion123")};

            var httpClient = new HttpClient(clientHandler);
            var mensagemParaEnviar = new ListaProcessoDeCotacaoDeFreteFechamento();

            var processoAuxiliar = (ProcessoDeCotacaoDeFrete) processo.CastEntity();

            foreach (var fornecedorParticipante in processoAuxiliar.FornecedoresParticipantes)
            {
                if (fornecedorParticipante.Cotacao != null && fornecedorParticipante.Cotacao.Selecionada)
                {
                    mensagemParaEnviar.Add(new ProcessoDeCotacaoDeFreteFechamentoVm
                        {
                            CodigoTransportadora = fornecedorParticipante.Fornecedor.Codigo,
                            CodigoMaterial = processoAuxiliar.Produto.Codigo,
                            CodigoUnidadeMedida = processoAuxiliar.UnidadeDeMedida.CodigoInterno,
                            CodigoItinerario = processoAuxiliar.Itinerario.Codigo,
                            DataDeValidadeInicial = processoAuxiliar.DataDeValidadeInicial.ToString("yyyyMMdd"),
                            DataDeValidaFinal = processoAuxiliar.DataDeValidadeFinal.ToString("yyyyMMdd"),
                            NumeroDoContrato = processoAuxiliar.NumeroDoContrato ?? "",
                            Valor = fornecedorParticipante.Cotacao.ValorComImpostos
                        });
                }
            }

            var response = httpClient.PostAsXmlAsync("http://sap-pid.bsbios.com:50000/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_cotacaoFreteFechamento_portal&qos=be"
                , mensagemParaEnviar);

            Stream content = response.Result.Content.ReadAsStreamAsync().Result;
            var serializer = new XmlSerializer(typeof(ApiResponseMessage));
            var mensagem = (ApiResponseMessage)serializer.Deserialize(content);
            if (mensagem.Retorno.Codigo == "E")
            {
                throw new ComunicacaoSapException("Ocorreu um erro ao comunicar o fechamento do Processo de Cotação de Frete para o SAP. Detalhes: " + mensagem.Retorno.Texto);
            }
            return mensagem;

        }
    }
}