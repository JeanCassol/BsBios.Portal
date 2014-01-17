using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using BsBios.Portal.Infra.Email;

namespace BsBios.Portal.GeradorDeRelatorio
{
    class Program
    {

        private static readonly string MensagemDeRodape = "Atenciosamente," + Environment.NewLine +
            "BSBIOS" + Environment.NewLine + Environment.NewLine +
            "Esta é uma mensagem gerada automaticamente, portanto, não deve ser respondida." + Environment.NewLine +
            "© BSBIOS. Todos os direitos reservados. Termos e Condições e Política de Privacidade." + Environment.NewLine;

        static void Main(string[] args)
        {
            var url = string.Format("http://{0}/RelatorioDeProcessoDeCotacaoDeFreteVisualizacao/GerarParaDownload", ConfigurationManager.AppSettings["HostDoServidor"]);
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    var streamReader = new StreamReader(responseStream);
                    var texto = streamReader.ReadToEnd();
                    var configuracaoDeDestinatarios = ConfigurationManager.AppSettings["Destinatarios"];

                    string[] destinatarios = configuracaoDeDestinatarios.Split(';');

                    if (destinatarios.Length == 0)
                    {
                        return;
                    }

                    var serializer = new JavaScriptSerializer();
                    var linkDoRelatorio = serializer.Deserialize<LinkDoRelatorio>(texto);

                    Console.WriteLine("Link do Relatório Aberto: " + linkDoRelatorio.LinkDoRelatorioAberto);
                    Console.WriteLine("Link do Relatório Fechado: " + linkDoRelatorio.LinkDoRelatorioFechado);

                    var mensagemDoCorpoDoEmail = "Prezado " + Environment.NewLine + Environment.NewLine +
                                                 "Segue link para fazer download dos relatórios de Processo de Cotação de Frete gerados em " +
                                                 DateTime.Today.ToShortDateString() + "." +
                                                 Environment.NewLine +
                                                 "Relatório Aberto --> " + linkDoRelatorio.LinkDoRelatorioAberto +
                                                 Environment.NewLine +
                                                 "Relatório Fechado --> " + linkDoRelatorio.LinkDoRelatorioFechado +
                                                 Environment.NewLine + Environment.NewLine +
                                                 MensagemDeRodape;

                    var mensagem =
                        new MensagemDeEmail(
                            "Relatorios de Processo de Cotação de Frete - " + DateTime.Today.ToShortDateString(),
                            mensagemDoCorpoDoEmail);

                    var emailDoPortal = ConfigurationManager.GetSection("emailDoPortal") as EmailDoPortal;

                    var contaDeEmail = new ContaDeEmail(
                        "Portal De Cotações <" + emailDoPortal.RemetenteLogistica + ">", emailDoPortal.Dominio,
                        emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                        emailDoPortal.Porta, emailDoPortal.HabilitarSsl);

                    var servicoDeEmail = new EmailService(contaDeEmail);

                    foreach (string destinatario in destinatarios)
                    {
                        servicoDeEmail.AdicionarDestinatario(destinatario);
                        Console.WriteLine("Adicionando destinatário " + destinatario);
                    }

                    servicoDeEmail.Enviar(mensagem);

                    Console.WriteLine("E-mail enviado com sucesso");
                    Thread.Sleep(5000);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Pressione uma tecla para continuar");
                Console.ReadKey();
            }
            finally
            {
                if (response != null)
                {
                    response.Close();    
                }
                
            }
        }
    }
}
