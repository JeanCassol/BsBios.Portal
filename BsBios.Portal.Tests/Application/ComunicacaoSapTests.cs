using System;
//using BsBios.Portal.Tests.ServiceReference1;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Serialization;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Application
{
    [TestClass]
    public class ComunicacaoSapTests
    {
        [TestMethod]
        public void ConsigoChamarUmWebServiceDoSap()
        {
            ExecutaChamadaAssincrona();
        }

        private /*async*/ void ExecutaChamadaAssincrona()
        {
            //var configuration = new HttpConfiguration();
            //configuration.Formatters.XmlFormatter.UseXmlSerializer = true;

            var clientHandler = new HttpClientHandler();
            clientHandler.Credentials = new NetworkCredential("fusion_lucas", "fusion123");

            var httpClient = new HttpClient(clientHandler);
            var mensagemParaEnviar = new ApiResponseMessage()
                {
                    Retorno = new Retorno()
                        {
                            Codigo = "100",
                            Texto = "blablabla"
                        }
                };

            var response = httpClient.PostAsXmlAsync("http://sap-pid.bsbios.com:50000/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_envioStatus_portal&qos=be"
                , mensagemParaEnviar);
            Assert.IsTrue(response.Result.IsSuccessStatusCode);
            //var retorno = response.Result.Content.ReadAsAsync<ApiResponseMessage>();
            //var retorno = response.Result.Content.ReadAsStringAsync();
            Stream content = response.Result.Content.ReadAsStreamAsync().Result;
            var serializer = new XmlSerializer(typeof(ApiResponseMessage)/*,"http://schemas.datacontract.org/2004/07/BsBios.Portal.ViewModel"*/);
            var responseMessage = (ApiResponseMessage)serializer.Deserialize(content);
            //var responseMessage = retorno.Result;
            Assert.IsNotNull(responseMessage);
            Assert.AreEqual("24", responseMessage.Retorno.Codigo);

             //*await*/ httpClient.PostAsXmlAsync("http://sap-pid.bsbios.com:50000/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_envioStatus_portal&qos=be"
             //   , mensagemParaEnviar).ContinueWith(
             //   requestTask =>
             //       {
             //           Console.WriteLine("requestTask");
             //           HttpResponseMessage response = requestTask.Result;
             //           response.EnsureSuccessStatusCode();

             //           response.Content.ReadAsAsync<ApiResponseMessage>().ContinueWith(
             //               readTask =>
             //                   {
             //                       Console.WriteLine("readTask");
             //                       Assert.AreEqual("200", readTask.Result.Retorno.Codigo);
             //                   });
             //       });
            
            Console.WriteLine("Fim do Teste");
        }

        //[TestMethod]
        //public void LerRespostaDeRequisicaoComRetornoEmXmlELeituraTipada()
        //{
        //    var clientHandler = new HttpClientHandler();
        //    clientHandler.Credentials = new NetworkCredential("fusion_lucas", "fusion123");

        //    var httpClient = new HttpClient(clientHandler);
        //    var mensagemParaEnviar = new ApiResponseMessage()
        //    {
        //        Retorno = new Retorno()
        //        {
        //            Codigo = "100",
        //            Texto = "blablabla"
        //        }
        //    };

        //    var response = httpClient.PostAsXmlAsync("http://sap-pid.bsbios.com:50000/HttpAdapter/HttpMessageServlet?senderParty=PORTAL&senderService=HTTP&interfaceNamespace=http://portal.bsbios.com.br/&interface=si_envioStatus_portal&qos=be"
        //        , mensagemParaEnviar);
        //    Assert.IsTrue(response.Result.IsSuccessStatusCode);
        //    var retorno = response.Result.Content.ReadAsAsync<ApiResponseMessage>();
        //    var responseMessage = retorno.Result;
        //    Assert.IsNotNull(responseMessage);
        //    Assert.AreEqual("24", responseMessage.Retorno.Codigo);
            
        //}

        [TestMethod]
        public void SerializarObjetoEmXml()
        {
            Stream stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(ApiResponseMessage));
            serializer.Serialize(stream, new ApiResponseMessage()
                {
                    Retorno = new Retorno()
                        {
                            Codigo = "101",
                            Texto = "testando"
                        }
                });
            
            Assert.IsTrue(stream.Length > 0);
            var streamReader = new StreamReader(stream);
            stream.Position = 0;
            string xmlText = streamReader.ReadToEnd();
            Assert.IsFalse(string.IsNullOrEmpty(xmlText));

            var serializer2 = new XmlSerializer(typeof(ApiResponseMessage));
            stream.Position = 0;
            var responseMessage = (ApiResponseMessage)serializer2.Deserialize(stream);
            Assert.IsNotNull(responseMessage);
            Assert.AreEqual("101", responseMessage.Retorno.Codigo);
        }

        [TestMethod]
        public void LerXmlComNamespaceDiferente()
        {
            Stream stream = new MemoryStream();
            var streamWritter = new StreamWriter(stream);
            streamWritter.Write("<?xml version=\"1.0\"?> <ns1:ApiResponseMessage xmlns:ns1=\"http://schemas.datacontract.org/2004/07/BsBios.Portal.ViewModel\"><ns1:Retorno><ns1:Codigo>24</ns1:Codigo><ns1:Texto>1 (s) entradas modificadas no ERP - ZNFIN_PROCTP_CAD</ns1:Texto></ns1:Retorno></ns1:ApiResponseMessage>");
            //streamWritter.Write("<?xml version=\"1.0\"?> <ApiResponseMessage xmlns=\"http://schemas.datacontract.org/2004/07/BsBios.Portal.ViewModel\"><Retorno><Codigo>24</Codigo><Texto>1 (s) entradas modificadas no ERP - ZNFIN_PROCTP_CAD</Texto></Retorno></ApiResponseMessage>");
            //streamWritter.Write("<?xml version=\"1.0\"?> <ApiResponseMessage><Retorno><Codigo>24</Codigo><Texto>1 (s) entradas modificadas no ERP - ZNFIN_PROCTP_CAD</Texto></Retorno></ApiResponseMessage>");
            //streamWritter.Write("<ApiResponseMessage><Retorno><Codigo>24</Codigo><Texto>1 (s) entradas modificadas no ERP - ZNFIN_PROCTP_CAD</Texto></Retorno></ApiResponseMessage>");
            //streamWritter.Write("<?xml version=\"1.0\"?><ApiResponseMessage xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><Retorno><Codigo>24</Codigo><Texto>1 (s) entradas modificadas no ERP - ZNFIN_PROCTP_CAD</Texto></Retorno></ApiResponseMessage>");
            streamWritter.Flush();
            stream.Position = 0;
            Assert.IsTrue(stream.Length > 0);
            var serializer = new XmlSerializer(typeof(ApiResponseMessage)/*, "http://schemas.datacontract.org/2004/07/BsBios.Portal.ViewModel"*/);
            var responseMessage = (ApiResponseMessage)serializer.Deserialize(stream);
            Assert.IsNotNull(responseMessage);
            Assert.AreEqual("24", responseMessage.Retorno.Codigo);
            
        }
    }
}
