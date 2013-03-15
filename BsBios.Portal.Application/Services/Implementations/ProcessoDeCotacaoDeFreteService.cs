using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
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
        public ProcessoDeCotacaoDeFreteService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IUnidadesDeMedida unidadesDeMedida, IItinerarios itinerarios, IProdutos produtos)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _unidadesDeMedida = unidadesDeMedida;
            _itinerarios = itinerarios;
            _produtos = produtos;
        }

        public void Salvar(ProcessoCotacaoFreteCadastroVm processoCotacaoFreteCadastroVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                UnidadeDeMedida unidadeDeMedida = _unidadesDeMedida.BuscaPeloCodigoInterno(processoCotacaoFreteCadastroVm.CodigoUnidadeMedida).Single();
                Itinerario itinerario = _itinerarios.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoItinerario).Single();
                Produto produto = _produtos.BuscaPeloCodigo(processoCotacaoFreteCadastroVm.CodigoMaterial);

                ProcessoDeCotacaoDeFrete processo;
                if (processoCotacaoFreteCadastroVm.Id.HasValue)
                {
                    processo = (ProcessoDeCotacaoDeFrete) _processosDeCotacao.BuscaPorId(processoCotacaoFreteCadastroVm.Id.Value).Single();
                    processo.Atualizar(produto, processoCotacaoFreteCadastroVm.QuantidadeMaterial,
                        unidadeDeMedida, processoCotacaoFreteCadastroVm.Requisitos, processoCotacaoFreteCadastroVm.NumeroDoContrato,
                        Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataLimiteRetorno), Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoInicial),
                        Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoFinal), itinerario);
                }
                else
                {
                    processo = new ProcessoDeCotacaoDeFrete(produto, processoCotacaoFreteCadastroVm.QuantidadeMaterial,
                        unidadeDeMedida, processoCotacaoFreteCadastroVm.Requisitos,processoCotacaoFreteCadastroVm.NumeroDoContrato,
                        Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataLimiteRetorno), Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoInicial),
                        Convert.ToDateTime(processoCotacaoFreteCadastroVm.DataValidadeCotacaoFinal), itinerario);
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
