using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml.Serialization;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Services.Implementations
{

    public class ComunicacaoSap : IComunicacaoSap
    {
        private readonly CredencialSap _credencialSap;

        public ComunicacaoSap(CredencialSap credencialSap)
        {
            _credencialSap = credencialSap;
        }

        public ApiResponseMessage EnviarMensagem(string endereco, Object mensagem)
        {
            var clientHandler = new HttpClientHandler { Credentials = new NetworkCredential(_credencialSap.Usuario, _credencialSap.Senha) };

            var httpClient = new HttpClient(clientHandler);

            var response = httpClient.PostAsXmlAsync(_credencialSap.EnderecoDoServidor + endereco, mensagem);

            if (!response.Result.IsSuccessStatusCode)
            {
                string erro = response.Result.Content.ReadAsStringAsync().Result;
                throw new ComunicacaoSapException(erro);
            }

            Stream content = response.Result.Content.ReadAsStreamAsync().Result;
            var serializer = new XmlSerializer(typeof(ApiResponseMessage));
            return (ApiResponseMessage)serializer.Deserialize(content);
        }
    }
}