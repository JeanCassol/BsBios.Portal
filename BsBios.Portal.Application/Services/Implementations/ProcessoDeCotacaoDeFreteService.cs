using System;
using System.Globalization;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoDeFreteService : IProcessoDeCotacaoDeFreteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IUnidadesDeMedida _unidadesDeMedida;
        private readonly IItinerarios _itinerarios;
        private readonly IProdutos _produtos;
        private readonly IFornecedores _fornecedores;
        private readonly IMunicipios _municipios;
        public ProcessoDeCotacaoDeFreteService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IUnidadesDeMedida unidadesDeMedida, 
            IItinerarios itinerarios, IProdutos produtos, IFornecedores fornecedores, IMunicipios municipios)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _unidadesDeMedida = unidadesDeMedida;
            _itinerarios = itinerarios;
            _produtos = produtos;
            _fornecedores = fornecedores;
            _municipios = municipios;
        }

        public void Salvar(ProcessoCotacaoFreteCadastroVm processoCotacaoFreteCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                UnidadeDeMedida unidadeDeMedida = _unidadesDeMedida.BuscaPeloCodigoInterno(processoCotacaoFreteCadastroVm.CodigoUnidadeMedida).Single();
                Itinerario itinerario = _itinerarios.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoItinerario).Single();
                Produto produto = _produtos.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoMaterial);
                Municipio municipioOrigem = _municipios.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoDoMunicipioDeOrigem);
                Municipio municipioDestino = _municipios.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoDoMunicipioDeDestino);

                Fornecedor fornecedor = null;

                if (!string.IsNullOrEmpty(processoCotacaoFreteCadastroVm.CodigoFornecedor) )
                {
                    fornecedor = _fornecedores.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoFornecedor);
                }
                
                int cadencia = int.Parse(processoCotacaoFreteCadastroVm.Cadencia, NumberStyles.AllowThousands, CultureInfo.CurrentCulture);
                DateTime dataDeValidadeInicial = Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoInicial);
                DateTime dataDeValidadeFinal = Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoFinal);
                DateTime dataLimiteDeRetorno = Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataLimiteRetorno);

                ProcessoDeCotacaoDeFrete processo;
                if (processoCotacaoFreteCadastroVm.Id.HasValue)
                {
                    processo = (ProcessoDeCotacaoDeFrete) _processosDeCotacao.BuscaPorId(processoCotacaoFreteCadastroVm.Id.Value).Single();
                    processo.Atualizar(produto, processoCotacaoFreteCadastroVm.QuantidadeMaterial,
                        unidadeDeMedida, processoCotacaoFreteCadastroVm.Requisitos, processoCotacaoFreteCadastroVm.NumeroDoContrato,
                        dataLimiteDeRetorno, dataDeValidadeInicial, dataDeValidadeFinal, itinerario, fornecedor, cadencia, 
                        processoCotacaoFreteCadastroVm.Classificacao,municipioOrigem, municipioDestino);
                }
                else
                {
                    processo = new ProcessoDeCotacaoDeFrete(produto, processoCotacaoFreteCadastroVm.QuantidadeMaterial,
                        unidadeDeMedida, processoCotacaoFreteCadastroVm.Requisitos,processoCotacaoFreteCadastroVm.NumeroDoContrato,
                        dataLimiteDeRetorno, dataDeValidadeInicial, dataDeValidadeFinal, itinerario, fornecedor, cadencia, 
                        processoCotacaoFreteCadastroVm.Classificacao, municipioOrigem, municipioDestino);
                }

                _processosDeCotacao.Save(processo);

                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
