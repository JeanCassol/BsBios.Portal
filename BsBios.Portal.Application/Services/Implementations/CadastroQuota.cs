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

        public CadastroQuota(IUnitOfWork unitOfWork, IQuotas quotas, IFornecedores fornecedores)
        {
            _unitOfWork = unitOfWork;
            _quotas = quotas;
            _fornecedores = fornecedores;
        }

        public void Salvar(IList<QuotaSalvarVm> quotasSalvarVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                //como todas as quotas são da mesma data posso pegar a data do primeiro elemento
                var dataDasQuotas = quotasSalvarVm.First().Data;
                //consulta as quotas que estão salvas na data
                IList<Quota> quotasSalvas = _quotas.FiltraPorData(dataDasQuotas).List();

                #region Remover Quotas
                IList<Quota> quotasParaRemover = quotasSalvas
                    .Where(qs => quotasSalvarVm.All(qc => 
                        qc.Data != qs.Data 
                        || qc.CodigoTerminal != qs.CodigoTerminal
                        || qc.CodigoMaterial != (int) qs.Material
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
                    .Where(qs => quotasSalvarVm.Any(qc =>
                        qc.Data == qs.Data
                        && qc.CodigoTerminal == qs.CodigoTerminal
                        && qc.CodigoMaterial == (int)qs.Material
                        && qc.CodigoFornecedor == qs.Fornecedor.Codigo)).ToList();

                foreach (var quota in quotasParaAtualizar)
                {
                    //obtem view model corresponde a entidade que quero atualizar
                    QuotaSalvarVm quotaSalvarVm = quotasSalvarVm.First(qa =>
                        qa.Data == quota.Data
                        && qa.CodigoTerminal == quota.CodigoTerminal
                        && qa.CodigoMaterial == (int)quota.Material
                        && qa.CodigoFornecedor == quota.Fornecedor.Codigo);

                    //único campo que pode ser atualizado é o campo de peso

                    quota.AlterarPeso(quotaSalvarVm.Peso);

                    _quotas.Save(quota);

                }
                #endregion

                #region Adicionar Quotas
                IList<QuotaSalvarVm> quotasParaAdicionar = quotasSalvarVm.Where(qc =>
                    quotasSalvas.All(qs =>
                        qc.Data != qs.Data 
                        || qc.CodigoTerminal != qs.CodigoTerminal
                        || qc.CodigoMaterial != (int) qs.Material
                        || qc.CodigoFornecedor != qs.Fornecedor.Codigo)).ToList();


                foreach (var quotaSalvarVm in quotasParaAdicionar)
                {
                    string[] codigoDosNovosFornecedores = quotasParaAdicionar.Select(x => x.CodigoFornecedor).Distinct().ToArray();
                    IList<Fornecedor> fornecedores = _fornecedores.BuscaListaPorCodigo(codigoDosNovosFornecedores).List();

                    var materialDeCarga =
                        (Enumeradores.MaterialDeCarga)
                        Enum.Parse(typeof (Enumeradores.MaterialDeCarga),Convert.ToString(quotaSalvarVm.CodigoMaterial));
                    Fornecedor fornecedor = fornecedores.First(x => x.Codigo == quotaSalvarVm.CodigoFornecedor);
                    var quota = new Quota(materialDeCarga, fornecedor, quotaSalvarVm.CodigoTerminal,
                                          quotaSalvarVm.Data, quotaSalvarVm.Peso);

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
