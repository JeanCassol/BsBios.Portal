using System.Diagnostics;
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
    public class ComunicacaoFechamentoProcessoCotacaoFreteOld : IComunicacaoSap
    {
        private readonly IComunicacaoSap<ListaProcessoDeCotacaoDeFreteFechamento> _comunicacaoSap;

        public ComunicacaoFechamentoProcessoCotacaoFreteOld(CredencialSap credencialSap)
        {
            _comunicacaoSap = comunicacaoSap;
        }

        private static void SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            StringWriter writer = new StringWriter();

            serializer.Serialize(writer, obj);

            Debug.WriteLine(writer.ToString());
        }


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

            if ( processo.FornecedoresSelecionados.Count == 0)
            {
                throw new ProcessoDeCotacaoFecharSemCotacaoSelecionadaException();
            }

            var clientHandler = new HttpClientHandler {Credentials = new NetworkCredential(_credencialSap.Usuario, _credencialSap.Senha)};

            var mensagemParaEnviar = new ListaProcessoDeCotacaoDeFreteFechamento();

            var processoDeCotacaoDeFrete = (ProcessoDeCotacaoDeFrete) processo.CastEntity();

            foreach (var fornecedorParticipante in processoDeCotacaoDeFrete.FornecedoresParticipantes)
            {
                if (fornecedorParticipante.Cotacao != null && fornecedorParticipante.Cotacao.Itens.First().Selecionada)
                {
                    CotacaoItem itemDaCotacao = fornecedorParticipante.Cotacao.Itens.First();
                    mensagemParaEnviar.Add(new ProcessoDeCotacaoDeFreteFechamentoComunicacaoSapVm
                        {
                            NumeroDoProcessoDeCotacao = processoDeCotacaoDeFrete.Id,
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

            SerializeToString(mensagemParaEnviar);

            var response = httpClient.PostAsXmlAsync(_credencialSap.EnderecoDoServidor + 
                "/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_cotacaoFreteFechamento_portal&qos=be"
                , mensagemParaEnviar);

            if (!response.Result.IsSuccessStatusCode)
            {
                string erro = response.Result.Content.ReadAsStringAsync().Result;
                throw new ComunicacaoSapException(response.Result.Content.Headers.ContentType.MediaType, erro);
            }

            const string mensagemDaExcecao = "Ocorreu um erro ao comunicar o fechamento do Processo de Cotação de Frete para o SAP. Detalhes: ";

            Stream content = response.Result.Content.ReadAsStreamAsync().Result;
            var serializer = new XmlSerializer(typeof(ProcessoDeCotacaoDeFreteFechamentoRetorno));

            var mensagem = (ProcessoDeCotacaoDeFreteFechamentoRetorno)serializer.Deserialize(content);
            
            if (mensagem.Retorno.Codigo == "E")
            {
                throw new ComunicacaoSapException("json",mensagemDaExcecao + mensagem.Retorno.Texto);
            }

            return mensagem;

        }
    }

    public class ComunicacaoFechamentoProcessoCotacaoFrete: IComunicacaoSap
    {
        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacao processo)
        {
            return new ApiResponseMessage
            {
                Retorno = new Retorno
                {
                    Codigo = "200",
                    Texto = "S"
                }
            };

        }
    }
}