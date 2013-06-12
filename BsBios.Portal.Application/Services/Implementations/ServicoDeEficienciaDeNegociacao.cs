using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ServicoDeEficienciaDeNegociacao : IServicoDeEficienciaDeNegociacao
    {
        private readonly IConsultaProcessoDeCotacaoDeMaterial _consultaProcessoDeCotacaoDeMaterial;

        public ServicoDeEficienciaDeNegociacao(IConsultaProcessoDeCotacaoDeMaterial consultaProcessoDeCotacaoDeMaterial)
        {
            _consultaProcessoDeCotacaoDeMaterial = consultaProcessoDeCotacaoDeMaterial;
        }

        public IList<dynamic> CalcularEficienciaDoItemDoProcesso(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            IList<FornecedorCotacaoVm> cotacoes = _consultaProcessoDeCotacaoDeMaterial.CotacoesDetalhadaDosFornecedores(idProcessoCotacao, idProcessoCotacaoItem);

            int numeroMaximoDeHistoricos = cotacoes.Select(x => x.Precos.Count()).Max();

            IList<dynamic> listaDinamica = new List<dynamic>();

            //dynamic data;
            IDictionary<string, object> dictionary;

            //cálculo das linhas de cotações
            for (int i = 0; i < numeroMaximoDeHistoricos; i++)
            {
                //data = new ExpandoObject();
                //dictionary = (IDictionary<string, object>)data;
                dictionary = new Dictionary<string, object> {{"Fornecedor", "Cotação " + Convert.ToString(i + 1)}};

                foreach (FornecedorCotacaoVm fornecedorCotacaoVm in cotacoes)
                {
                    if (i < fornecedorCotacaoVm.Precos.Count())
                    {
                        dictionary.Add("F" + fornecedorCotacaoVm.Codigo,  fornecedorCotacaoVm.Precos[i]);    
                    }
                }

                listaDinamica.Add(dictionary);

            }

            ////calculo da eficiência em percentual (gera um linha)
            var dictionaryPercentualDeEficiencia = new Dictionary<string, object>();
            dictionaryPercentualDeEficiencia.Add("Fornecedor","Eficiência de Negociação (%)");

            var dictionaryValorUnitarioDeEficiencia = new Dictionary<string, object>();
            dictionaryValorUnitarioDeEficiencia.Add("Fornecedor","Eficiência de Negociação (R$)");

            var dictionaryValorTotalDeEficiencia = new Dictionary<string, object>();
            dictionaryValorTotalDeEficiencia.Add("Fornecedor","Eficiência de Negociação Total (R$)");

            IList<FornecedorCotacaoVm> cotacoesSelecionadas = cotacoes.Where(x => x.Selecionada).ToList();

            foreach (FornecedorCotacaoVm cotacaoSelecionada in cotacoesSelecionadas)
            {
                dictionaryPercentualDeEficiencia.Add("F" + cotacaoSelecionada.Codigo,
                    Math.Round(((cotacaoSelecionada.PrecoInicial - cotacaoSelecionada.PrecoFinal) / cotacaoSelecionada.PrecoInicial) * 100,2));

                dictionaryValorUnitarioDeEficiencia.Add("F" + cotacaoSelecionada.Codigo, cotacaoSelecionada.PrecoInicial - cotacaoSelecionada.PrecoFinal);

                dictionaryValorTotalDeEficiencia.Add("F" + cotacaoSelecionada.Codigo, 
                    (cotacaoSelecionada.PrecoInicial - cotacaoSelecionada.PrecoFinal) * cotacaoSelecionada.QuantidadeAdquirida);
            }

            listaDinamica.Add(dictionaryPercentualDeEficiencia);
            listaDinamica.Add(dictionaryValorUnitarioDeEficiencia);
            listaDinamica.Add(dictionaryValorTotalDeEficiencia);
            
            return listaDinamica;

        }
    }
}
