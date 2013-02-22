using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using StructureMap;

namespace BsBios.Portal.Tests.DefaultProvider
{
    public static class DefaultPersistedObjects
    {
        private static readonly IUnitOfWorkNh UnitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        public static void PersisteUsuario(Usuario usuario)
        {
            Queries.RemoverUsuariosCadastrados();
            UnitOfWorkNh.BeginTransaction();
            var usuarios = ObjectFactory.GetInstance<IUsuarios>();
            usuarios.Save(usuario);
            UnitOfWorkNh.Commit();
        }

        public static void PersistirUsuarios(IList<Usuario> usuariosCriados)
        {
            Queries.RemoverUsuariosCadastrados();
            UnitOfWorkNh.BeginTransaction();
            var usuarios = ObjectFactory.GetInstance<IUsuarios>();
            foreach (var usuarioCriado in usuariosCriados)
            {
                usuarios.Save(usuarioCriado);
            }

            UnitOfWorkNh.Commit();
        }

        public static void PersistirFornecedor(Fornecedor fornecedor)
        {
            Queries.RemoverFornecedoresCadastrados();
            UnitOfWorkNh.BeginTransaction();
            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            fornecedores.Save(  fornecedor);
            UnitOfWorkNh.Commit();
        }

        public static void PersistirProduto(Produto produto)
        {
            Queries.RemoverProdutosCadastrados();
            UnitOfWorkNh.BeginTransaction();
            var produtos = ObjectFactory.GetInstance<IProdutos>();
            produtos.Save(produto);
            UnitOfWorkNh.Commit();
        }

        public static void PersistirRequisicaoDeCompra(RequisicaoDeCompra requisicaoDeCompra)
        {
            Queries.RemoverRequisicoesDeCompraCadastradas();
            PersistirUsuarios(new List<Usuario>(){requisicaoDeCompra.Criador, requisicaoDeCompra.Requisitante});
            PersistirFornecedor(requisicaoDeCompra.FornecedorPretendido);
            PersistirProduto(requisicaoDeCompra.Material);

            UnitOfWorkNh.BeginTransaction();
            var requisicoesDeCompra = ObjectFactory.GetInstance<IRequisicoesDeCompra>();
            requisicoesDeCompra.Save(requisicaoDeCompra);
            UnitOfWorkNh.Commit();
        }
    }
}
