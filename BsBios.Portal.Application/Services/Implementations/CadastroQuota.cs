using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CadastroQuota: ICadastroQuota
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotas _quotas;
        private readonly IFornecedores _fornecedores;
        private readonly ITerminais _terminais;

        public CadastroQuota(IUnitOfWork unitOfWork, IQuotas quotas, IFornecedores fornecedores, ITerminais terminais)
        {
            _unitOfWork = unitOfWork;
            _quotas = quotas;
            _fornecedores = fornecedores;
            _terminais = terminais;
        }

        public void Salvar(QuotasSalvarVm quotasSalvarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                //consulta as quotas que estão salvas na data
                IList<Quota> quotasSalvas = _quotas
                    .FiltraPorData(quotasSalvarVm.Data)
                    .DoTerminal(quotasSalvarVm.CodigoDoTerminal)
                    .List();

                #region Remover Quotas
                IList<Quota> quotasParaRemover = quotasSalvas
                    .Where(qs => quotasSalvarVm.Quotas.All(qc => 
                        qc.CodigoMaterial != (int) qs.Material
                        || qc.CodigoFornecedor != qs.Fornecedor.Codigo)).ToList();

                foreach (var quota in quotasParaRemover)
                {
                    if (quota.Agendamentos.Any())
                    {
                        throw new ExcluirQuotaComAgendamentoException(quota.Fornecedor.Nome);
                    }
                    _quotas.Delete(quota);
                }
                #endregion

                #region Atualizar Quotas
                IList<Quota> quotasParaAtualizar = quotasSalvas
                    .Where(qs => quotasSalvarVm.Quotas.Any(qc =>
                        qc.CodigoMaterial == (int)qs.Material
                        && qc.CodigoFornecedor == qs.Fornecedor.Codigo)).ToList();

                foreach (var quota in quotasParaAtualizar)
                {
                    //obtem view model corresponde a entidade que quero atualizar
                    QuotaSalvarVm quotaSalvarVm = quotasSalvarVm.Quotas.First(qa =>
                        qa.CodigoMaterial == (int)quota.Material
                        && qa.CodigoFornecedor == quota.Fornecedor.Codigo);

                    //único campo que pode ser atualizado é o campo de peso

                    quota.AlterarPeso(quotaSalvarVm.Peso);

                    _quotas.Save(quota);

                }
                #endregion

                #region Adicionar Quotas
                IList<QuotaSalvarVm> quotasParaAdicionar = quotasSalvarVm.Quotas.Where(qc =>
                    quotasSalvas.All(qs =>
                        qc.CodigoMaterial != (int) qs.Material
                        || qc.CodigoFornecedor != qs.Fornecedor.Codigo)).ToList();

                Terminal terminal = null;
                if (quotasParaAdicionar.Any())
                {
                    terminal = _terminais.BuscaPeloCodigo(quotasSalvarVm.CodigoDoTerminal);
                }
                
                foreach (var quotaSalvarVm in quotasParaAdicionar)
                {
                    string[] codigoDosNovosFornecedores = quotasParaAdicionar.Select(x => x.CodigoFornecedor).Distinct().ToArray();
                    IList<Fornecedor> fornecedores = _fornecedores.BuscaListaPorCodigo(codigoDosNovosFornecedores).List();

                    var materialDeCarga =
                        (Enumeradores.MaterialDeCarga)
                        Enum.Parse(typeof (Enumeradores.MaterialDeCarga),Convert.ToString(quotaSalvarVm.CodigoMaterial));
                    Fornecedor fornecedor = fornecedores.First(x => x.Codigo == quotaSalvarVm.CodigoFornecedor);
                    var quota = new Quota(materialDeCarga, fornecedor, terminal,
                                          quotasSalvarVm.Data, quotaSalvarVm.Peso);

                    _quotas.Save(quota);
                }
                #endregion

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
