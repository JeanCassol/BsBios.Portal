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
    public class ProcessoDeCotacaoDeFreteFechamentoComunicacaoSap : IProcessoDeCotacaoDeFreteFechamentoComunicacaoSap
    {
        //private readonly IComunicacaoSap<ListaProcessoDeCotacaoDeFreteFechamento> _comunicacaoSap;
        private readonly CredencialSap _credencialSap;

        public ProcessoDeCotacaoDeFreteFechamentoComunicacaoSap(CredencialSap credencialSap
/*IComunicacaoSap<ListaProcessoDeCotacaoDeFreteFechamento> comunicacaoSap*/)
        {
            _credencialSap = credencialSap;
            //_comunicacaoSap = comunicacaoSap;
        }

        public ApiResponseMessage EfetuarComunicacao(ProcessoDeCotacaoDeFrete processo)
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

            if (processo.FornecedoresSelecionados.Count == 0)
            {
                throw new ProcessoDeCotacaoFecharSemCotacaoSelecionadaException();
            }

            var mensagemParaEnviar = new ListaProcessoDeCotacaoDeFreteFechamento();

            var processoDeCotacaoDeFrete = processo.CastEntity();

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
                        CodigoMaterial = item.Produto.Codigo,
                        //CodigoUnidadeMedida = processoAuxiliar.UnidadeDeMedida.CodigoInterno,
                        CodigoUnidadeMedida = item.UnidadeDeMedida.CodigoInterno,
                        CodigoItinerario = processo.Itinerario.Codigo,
                        DataDeValidadeInicial = processo.DataDeValidadeInicial.ToString("yyyyMMdd"),
                        DataDeValidaFinal = processo.DataDeValidadeFinal.ToString("yyyyMMdd"),
                        NumeroDoContrato = processo.NumeroDoContrato ?? "",
                        Valor = itemDaCotacao.ValorComImpostos
                    });
                }
            }

            SerializeToString(mensagemParaEnviar);

            var clientHandler = new HttpClientHandler { Credentials = new NetworkCredential(_credencialSap.Usuario, _credencialSap.Senha) };

            var httpClient = new HttpClient(clientHandler);

            var response = httpClient.PostAsXmlAsync(_credencialSap.EnderecoDoServidor +
                                                     "/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_cotacaoFreteFechamento_portal&qos=be"
                , mensagemParaEnviar);

            if (!response.Result.IsSuccessStatusCode)
            {
                string erro = response.Result.Content.ReadAsStringAsync().Result;
                throw new ComunicacaoSapException(response.Result.Content.Headers.ContentType.MediaType, erro);
            }
            //ApiResponseMessage apiResponseMessage =
            //    _comunicacaoSap.EnviarMensagem(
            //        "/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_cotacaoFreteFechamento_portal&qos=be"
            //        , mensagemParaEnviar);
            //if (apiResponseMessage.Retorno.Codigo == "E")
            //{
            //    throw new ComunicacaoSapException("json", "Ocorreu um erro ao comunicar o fechamento do Processo de Cotação de Frete para o SAP. Detalhes: " + apiResponseMessage.Retorno.Texto);
            //}
            //return apiResponseMessage;

            const string mensagemDaExcecao = "Ocorreu um erro ao comunicar o fechamento do Processo de Cotação de Frete para o SAP. Detalhes: ";

            Stream content = response.Result.Content.ReadAsStreamAsync().Result;
            var serializer = new XmlSerializer(typeof(ProcessoDeCotacaoDeFreteFechamentoRetorno));

            var mensagem = (ProcessoDeCotacaoDeFreteFechamentoRetorno)serializer.Deserialize(content);

            if (mensagem.Retorno.Codigo == "E")
            {
                throw new ComunicacaoSapException("json", mensagemDaExcecao + mensagem.Retorno.Texto);
            }

            return mensagem;

        }

        private static void SerializeToString(object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            StringWriter writer = new StringWriter();

            serializer.Serialize(writer, obj);

        }
    }
}