using System;
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
    }
}
