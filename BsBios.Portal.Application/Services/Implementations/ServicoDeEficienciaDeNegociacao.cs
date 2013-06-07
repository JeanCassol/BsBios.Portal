using System.Collections.Generic;
using System.Dynamic;
using BsBios.Portal.Application.Services.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ServicoDeEficienciaDeNegociacao : IServicoDeEficienciaDeNegociacao
    {
        public string[] ListarFornecedores(string numeroDaRequisicao, string numeroDoItem)
        {
            if (numeroDoItem == "ITEM001")
            {
                return new[] { "Madrilins", "Rolate", "Rodol", "Videl", "Paludo" };
            }

            return new[] { "Arlins", "Proar", "Aclimar", "	Aclipasso", "Soar" };

        }

        private IList<dynamic> CalculaEficienciaItem001()
        {
            IList<dynamic> listaDinamica = new List<dynamic>();

            dynamic data;
            IDictionary<string, object> dictionary;

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Cotação 1");
            dictionary.Add("Madrilins", 1000);
            dictionary.Add("Rolate", 1050);
            dictionary.Add("Rodol", 1010);
            dictionary.Add("Videl", 990);
            dictionary.Add("Paludo", 1000);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Cotação 2");
            dictionary.Add("Madrilins", 995);
            dictionary.Add("Rolate", 1000);
            dictionary.Add("Rodol", 1000);
            dictionary.Add("Videl", 980);
            dictionary.Add("Paludo", 980);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Cotação 3");
            dictionary.Add("Rolate", 995);
            dictionary.Add("Rodol", 980);
            dictionary.Add("Videl", 975);
            dictionary.Add("Paludo", 950);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Cotação 4");
            dictionary.Add("Rolate", 900);
            dictionary.Add("Videl", 950);
            dictionary.Add("Paludo", 900);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Preço Negociado");
            dictionary.Add("Rolate", 900);
            dictionary.Add("Videl", 900);
            dictionary.Add("Paludo", 850);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Eficiencia de Negociação (%)");
            dictionary.Add("Rolate", 10.45);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Eficiencia de Negociação (R$)");
            dictionary.Add("Rolate", 150);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Eficiencia de Negociação Total(R$)");
            dictionary.Add("Rolate", 30000);

            listaDinamica.Add(dictionary);

            return listaDinamica;
        }

        private IList<dynamic> CalculaEficienciaItem002()
        {
            IList<dynamic> listaDinamica = new List<dynamic>();

            dynamic data;
            IDictionary<string, object> dictionary;

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Cotação 1");
            dictionary.Add("Arlins", 30000);
            dictionary.Add("Proar", 32000);
            dictionary.Add("Aclimar", 50000);
            dictionary.Add("Aclipasso", 18000);
            dictionary.Add("Soar", 18000);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Cotação 2");
            dictionary.Add("Soar", 17500);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Preço Negociado");
            dictionary.Add("Soar", 17000);
            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Eficiencia de Negociação (%)");
            dictionary.Add("Soar", 5.55);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Eficiencia de Negociação (R$)");
            dictionary.Add("Soar", 1000);

            listaDinamica.Add(dictionary);

            data = new ExpandoObject();
            dictionary = (IDictionary<string, object>)data;

            dictionary.Add("Fornecedor", "Eficiencia de Negociação Total(R$)");
            dictionary.Add("Soar", 1000);

            listaDinamica.Add(dictionary);

            return listaDinamica;
        }


        public IList<dynamic> CalcularEficienciaDoItemDoProcesso(string numeroDaRequisicao, string numeroDoItem)
        {
            if (numeroDoItem == "ITEM001")
            {
                return CalculaEficienciaItem001();
            }
            return CalculaEficienciaItem002();

        }
    }
}
