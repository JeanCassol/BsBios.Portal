using System;
using System.Collections;
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
    public class ProcessoDeCotacaoSelecaoService : IProcessoDeCotacaoSelecaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IIvas _ivas;

        public ProcessoDeCotacaoSelecaoService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, IIvas ivas)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _ivas = ivas;
        }

        public void AtualizarSelecao(ProcessoDeCotacaoSelecaoAtualizarVm processoDeCotacaoSelecaoAtualizarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(processoDeCotacaoSelecaoAtualizarVm.IdProcessoCotacao).Single();
                string[] codigosIva = processoDeCotacaoSelecaoAtualizarVm.Cotacoes.Select(x => x.CodigoIva).ToArray();
                IList<Iva> ivasSelecionados = _ivas.BuscaListaPorCodigo(codigosIva).List();
                foreach (var cotacaoSelecaoVm in processoDeCotacaoSelecaoAtualizarVm.Cotacoes)
                {
                    var ivaSelecionado = ivasSelecionados.Single(x => x.Codigo == cotacaoSelecaoVm.CodigoIva);
                    if (cotacaoSelecaoVm.Selecionada)
                    {
                        processoDeCotacao.SelecionarCotacao(cotacaoSelecaoVm.IdCotacao, cotacaoSelecaoVm.QuantidadeAdquirida.Value,ivaSelecionado);
                    }
                    else
                    {
                        processoDeCotacao.RemoverSelecaoDaCotacao(cotacaoSelecaoVm.IdCotacao,ivaSelecionado);
                    }

                }

                _processosDeCotacao.Save(processoDeCotacao);

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
