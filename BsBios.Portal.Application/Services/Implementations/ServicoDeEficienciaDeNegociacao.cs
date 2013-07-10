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
        private readonly IConsultaEficienciaDeNegociacao _consultaEficienciaDeNegociacao;

        public ServicoDeEficienciaDeNegociacao(IConsultaProcessoDeCotacaoDeMaterial consultaProcessoDeCotacaoDeMaterial, IConsultaEficienciaDeNegociacao consultaEficienciaDeNegociacao)
        {
            _consultaProcessoDeCotacaoDeMaterial = consultaProcessoDeCotacaoDeMaterial;
            _consultaEficienciaDeNegociacao = consultaEficienciaDeNegociacao;
        }

        public FornecedorVm[] ListarFornecedores(int idProcessoCotacao)
        {
            return _consultaProcessoDeCotacaoDeMaterial.ListarFornecedores(idProcessoCotacao);
        }

        public KendoGridVm CalcularResumo(PaginacaoVm paginacaoVm, EficienciaNegociacaoFiltroVm filtro)
        {
            return _consultaEficienciaDeNegociacao.ConsultarResumo(paginacaoVm, filtro);
        }

        public IList<dynamic> CalcularEficienciaDoItemDoProcesso(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            IList<FornecedorCotacaoVm> cotacoes = _consultaProcessoDeCotacaoDeMaterial.CotacoesDetalhadaDosFornecedores(idProcessoCotacao, idProcessoCotacaoItem);

            int numeroMaximoDeHistoricos = cotacoes.Select(x => x.Precos.Count()).Max();

            IList<dynamic> listaDinamica = new List<dynamic>();

            //dynamic data;

            //cálculo das linhas de cotações
            for (int i = 0; i < numeroMaximoDeHistoricos; i++)
            {
                //data = new ExpandoObject();
                //dictionary = (IDictionary<string, object>)data;
                IDictionary<string, object> dictionary = new Dictionary<string, object> {{"Fornecedor", "Cotação " + Convert.ToString(i + 1)}};

                foreach (FornecedorCotacaoVm fornecedorCotacaoVm in cotacoes)
                {
                    if (i < fornecedorCotacaoVm.Precos.Count())
                    {
                        dictionary.Add("F" + fornecedorCotacaoVm.Codigo,  fornecedorCotacaoVm.Precos[i]);    
                    }
                }

                listaDinamica.Add(dictionary);

            }

            //calculo da eficiência em percentual (gera um linha)

            var dictionaryQuantidadeAdquirida = new Dictionary<string, object>()
                {
                    {"Fornecedor","Quantidade Adquirida"}
                };
            var dictionaryPercentualDeEficiencia = new Dictionary<string, object>
                {
                    {"Fornecedor", "Eficiência de Negociação (%)"}
                };

            var dictionaryValorUnitarioDeEficiencia = new Dictionary<string, object>
                {
                    {"Fornecedor", "Eficiência de Negociação (R$)"}
                };

            var dictionaryValorTotalDeEficiencia = new Dictionary<string, object>
                {
                    {"Fornecedor", "Eficiência de Negociação Total (R$)"}
                };

            IList<FornecedorCotacaoVm> cotacoesSelecionadas = cotacoes.Where(x => x.Selecionada).ToList();

            foreach (FornecedorCotacaoVm cotacaoSelecionada in cotacoesSelecionadas)
            {
                string codigoDoFornecedor = "F" + cotacaoSelecionada.Codigo;

                dictionaryQuantidadeAdquirida.Add(codigoDoFornecedor, cotacaoSelecionada.QuantidadeAdquirida);

                dictionaryPercentualDeEficiencia.Add(codigoDoFornecedor,
                    Math.Round(((cotacaoSelecionada.PrecoInicial - cotacaoSelecionada.PrecoFinal) / cotacaoSelecionada.PrecoInicial) * 100,2));

                dictionaryValorUnitarioDeEficiencia.Add(codigoDoFornecedor, cotacaoSelecionada.PrecoInicial - cotacaoSelecionada.PrecoFinal);

                dictionaryValorTotalDeEficiencia.Add(codigoDoFornecedor, 
                    (cotacaoSelecionada.PrecoInicial - cotacaoSelecionada.PrecoFinal) * cotacaoSelecionada.QuantidadeAdquirida);
            }

            listaDinamica.Add(dictionaryQuantidadeAdquirida);
            listaDinamica.Add(dictionaryPercentualDeEficiencia);
            listaDinamica.Add(dictionaryValorUnitarioDeEficiencia);
            listaDinamica.Add(dictionaryValorTotalDeEficiencia);
            
            return listaDinamica;

        }
    }
}
