using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class ComunicacaoFechamentoProcessoCotacaoFrete : IComunicacaoSap
    {
        private readonly CredencialSap _credencialSap;

        public ComunicacaoFechamentoProcessoCotacaoFrete(CredencialSap credencialSap)
        {
            _credencialSap = credencialSap;
        }

        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo)
        {
            ProcessoDeCotacaoItem item = processo.Itens.First();
            if (item.Produto.Tipo.ToUpper() == "NLAG")
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
            var clientHandler = new HttpClientHandler {Credentials = new NetworkCredential(_credencialSap.Usuario, _credencialSap.Senha)};

            var httpClient = new HttpClient(clientHandler);
            var mensagemParaEnviar = new ListaProcessoDeCotacaoDeFreteFechamento();

            var processoAuxiliar = (ProcessoDeCotacaoDeFrete) processo.CastEntity();

            foreach (var fornecedorParticipante in processoAuxiliar.FornecedoresParticipantes)
            {
                if (fornecedorParticipante.Cotacao != null && fornecedorParticipante.Cotacao.Selecionada)
                {
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
                            Valor = fornecedorParticipante.Cotacao.ValorComImpostos
                        });
                }
            }

            var response = httpClient.PostAsXmlAsync(_credencialSap.EnderecoDoServidor + 
                "/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_cotacaoFreteFechamento_portal&qos=be"
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