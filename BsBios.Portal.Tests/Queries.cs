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
            UnitOfWork.Session.CreateSQLQuery("DELETE FROM USUARIO").ExecuteUpdate();
        }

        public static void RemoverProdutosCadastrados()
        {
            UnitOfWork.Session.CreateSQLQuery("DELETE FROM PRODUTO").ExecuteUpdate();
        }

        public static void RemoverFornecedoresCadastrados()
        {
            UnitOfWork.Session.CreateSQLQuery("DELETE FROM FORNECEDOR").ExecuteUpdate();
        }

        public static void RemoverCondicoesDePagamentoCadastradas()
        {
            UnitOfWork.Session.CreateSQLQuery("DELETE FROM CONDICAOPAGAMENTO").ExecuteUpdate();
        }

        public static void RemoverIvasCadastrados()
        {
            UnitOfWork.Session.CreateSQLQuery("DELETE FROM IVA").ExecuteUpdate();
        }
    }
}
