using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados
{
    public static class RemoveQueries
    {
        private static readonly IUnitOfWorkNh UnitOfWork;
        static RemoveQueries()
        {
            UnitOfWork = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        }


        public static void RemoverUsuariosCadastrados()
        {
            try
            {
                RemoverProcessosDeCotacaoCadastrados();
                RemoverRequisicoesDeCompraCadastradas();

                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Usuario");
                UnitOfWork.Commit();
                UnitOfWork.Session.Clear();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();                
                throw;
            }
        }

        public static void RemoverProdutosCadastrados()
        {
            try
            {
                RemoverProcessosDeCotacaoCadastrados();
                RemoverRequisicoesDeCompraCadastradas();
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Produto");
                UnitOfWork.Commit();
                UnitOfWork.Session.Clear();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverFornecedoresCadastrados()
        {
            try
            {
                RemoverProcessosDeCotacaoCadastrados();
                RemoverRequisicoesDeCompraCadastradas();
                RemoverQuotasCadastradas();
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Fornecedor");
                UnitOfWork.Commit();
                UnitOfWork.Session.Clear();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverCondicoesDePagamentoCadastradas()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from CondicaoDePagamento");
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverIvasCadastrados()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Iva");
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();                
                throw;
            }
        }

        public static void RemoverRequisicoesDeCompraCadastradas()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from RequisicaoDeCompra");
                UnitOfWork.Commit();
                UnitOfWork.Session.Clear();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverProcessosDeCotacaoCadastrados()
        {
            try
            {
                UnitOfWork.BeginTransaction();

                var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
                processosDeCotacao.FiltraPorTipo(Enumeradores.TipoDeCotacao.Material);
                var cotacoesDeMaterial = processosDeCotacao.List();
                foreach (var processoDeCotacao in cotacoesDeMaterial)
                {
                    foreach (ProcessoDeCotacaoDeMaterialItem processoDeCotacaoItem in processoDeCotacao.Itens)
                    {
                        var requisicaoDeCompra = processoDeCotacaoItem.RequisicaoDeCompra;
                        if (requisicaoDeCompra != null)
                        {
                            requisicaoDeCompra.DesvincularDeProcessoDeCotacao();
                            UnitOfWork.Session.Save(requisicaoDeCompra);
                        }
                    }
                    UnitOfWork.Session.Delete(processoDeCotacao);
                }

                UnitOfWork.Commit();

                UnitOfWork.Session.Clear();

                UnitOfWork.Session.BeginTransaction();
                
                UnitOfWork.Session.Delete("from ProcessoDeCotacao");

                UnitOfWork.Commit();

                UnitOfWork.Session.Clear();

            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverIncotermsCadastrados()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Incoterm");
        
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverItinerariosCadastrados()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Itinerario");
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverUnidadesDeMedidaCadastradas()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from UnidadeDeMedida");
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverQuotasCadastradas()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Quota");
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

        public static void RemoverProcessoCotacaoIteracaoUsuarioCadastradas()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from ProcessoCotacaoIteracaoUsuario");
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();
                throw;
            }
        }

       
    }
}
