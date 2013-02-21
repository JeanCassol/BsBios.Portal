using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
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
                UnitOfWork.BeginTransaction();
                UnitOfWork.Session.Delete("from Usuario");
                UnitOfWork.Commit();
            }
            catch (Exception)
            {
                UnitOfWork.RollBack();                
                throw;
            }
        }

        //public static void RemoverProdutosCadastrados()
        //{
        //    try
        //    {
        //        var produtos = ObjectFactory.GetInstance<IProdutos>();
        //        UnitOfWork.BeginTransaction();
        //        var todosProdutos = produtos.List();
        //        foreach (var produto in todosProdutos)
        //        {
        //            produtos.Delete(produto);
        //        }
        //        UnitOfWork.Commit();

        //    }
        //    catch (Exception)
        //    {
        //        UnitOfWork.RollBack();                
        //        throw;
        //    }
        //}

        //public static void RemoverFornecedoresCadastrados()
        //{
        //    try
        //    {
        //        var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
        //        UnitOfWork.BeginTransaction();
        //        var todosFornecedores = fornecedores.List();
        //        foreach (var fornecedor in todosFornecedores)
        //        {
        //            fornecedores.Delete(fornecedor);
        //        } 
        //        UnitOfWork.Commit();
        //    }
        //    catch (Exception)
        //    {
        //        UnitOfWork.RollBack();
        //        throw;
        //    }
        //}

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
    }
}
