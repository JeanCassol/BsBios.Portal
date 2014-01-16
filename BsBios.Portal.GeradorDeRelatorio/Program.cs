using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.GeradorDeRelatorio
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = (HttpWebRequest) WebRequest.Create(string.Format("http://{0}/RelatorioParaDownload/Gerar",ConfigurationManager.AppSettings["HostDoServidor"]) );
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
                    Console.WriteLine(texto);
                    Console.ReadKey();
                }
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
