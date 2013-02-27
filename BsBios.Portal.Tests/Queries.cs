using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.Tests
{
    public static class Queries
    {
        private static readonly IUnitOfWorkNh UnitOfWork;
        static Queries()
        {
            UnitOfWork = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        }


        public static void RemoverUsuariosCadastrados()
        {
            try
            {
                RemoverProcessosDeCotacaoDeMateriaisCadastradas();
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
                RemoverProcessosDeCotacaoDeMateriaisCadastradas();
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
                RemoverProcessosDeCotacaoDeMateriaisCadastradas();
                RemoverRequisicoesDeCompraCadastradas();
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

        public static void RemoverProcessosDeCotacaoDeMateriaisCadastradas()
        {
            try
            {
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from ProcessoDeCotacaoDeMaterial");

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
    }
}
