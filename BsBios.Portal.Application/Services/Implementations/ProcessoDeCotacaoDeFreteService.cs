using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
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
        private readonly ITerminais _terminais;

        public ProcessoDeCotacaoDeFreteService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IUnidadesDeMedida unidadesDeMedida, 
            IItinerarios itinerarios, IProdutos produtos, IFornecedores fornecedores, IMunicipios municipios, ITerminais terminais)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _unidadesDeMedida = unidadesDeMedida;
            _itinerarios = itinerarios;
            _produtos = produtos;
            _fornecedores = fornecedores;
            _municipios = municipios;
            _terminais = terminais;
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

                Fornecedor fornecedorDaMercadoria = _fornecedores.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoDoFornecedorDaMercadoria);

                Fornecedor deposito = null;

                if (!string.IsNullOrEmpty(processoCotacaoFreteCadastroVm.CodigoDoDeposito))
                {
                    deposito = _fornecedores.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoDoDeposito);
                }
                
                decimal cadencia = processoCotacaoFreteCadastroVm.Cadencia;
                DateTime dataDeValidadeInicial = Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoInicial);
                DateTime dataDeValidadeFinal = Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoFinal);
                DateTime dataLimiteDeRetorno = Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataLimiteRetorno);

                Terminal terminal = _terminais.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoDoTerminal);

                ProcessoDeCotacaoDeFrete processo;
                if (processoCotacaoFreteCadastroVm.Id.HasValue)
                {
                    processo = (ProcessoDeCotacaoDeFrete) _processosDeCotacao.BuscaPorId(processoCotacaoFreteCadastroVm.Id.Value).Single();
                    processo.Atualizar(produto, processoCotacaoFreteCadastroVm.QuantidadeMaterial,
                        unidadeDeMedida, processoCotacaoFreteCadastroVm.Requisitos, processoCotacaoFreteCadastroVm.NumeroDoContrato,
                        dataLimiteDeRetorno, dataDeValidadeInicial, dataDeValidadeFinal, itinerario, fornecedorDaMercadoria, cadencia, 
                        processoCotacaoFreteCadastroVm.Classificacao,municipioOrigem, municipioDestino,deposito,terminal);
                }
                else
                {
                    processo = new ProcessoDeCotacaoDeFrete(produto, processoCotacaoFreteCadastroVm.QuantidadeMaterial,
                        unidadeDeMedida, processoCotacaoFreteCadastroVm.Requisitos,processoCotacaoFreteCadastroVm.NumeroDoContrato,
                        dataLimiteDeRetorno, dataDeValidadeInicial, dataDeValidadeFinal, itinerario, fornecedorDaMercadoria, cadencia, 
                        processoCotacaoFreteCadastroVm.Classificacao, municipioOrigem, municipioDestino, deposito,terminal);
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
