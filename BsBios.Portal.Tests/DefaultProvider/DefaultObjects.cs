using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;

namespace BsBios.Portal.Tests.DefaultProvider
{
    public static class DefaultObjects
    {
        public static RequisicaoDeCompra ObtemRequisicaoDeCompraPadrao()
        {
            var usuarioCriador = new Usuario("Usuario Criador", "criador", "", Enumeradores.Perfil.Comprador);
            var usuarioRequisitante = new Usuario("Usuario Requisitante", "requisitante", "", Enumeradores.Perfil.Comprador);
            var fornecedorPretendido = new Fornecedor("fpret", "Fornecedor Pretendido", null);
            var material = new Produto("MAT0001", "MATERIAL DE COMPRA", "T01");
            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, usuarioRequisitante, fornecedorPretendido,
                dataDeRemessa, dataDeLiberacao, dataDeSolicitacao, "CENTRO", "UNT", 1000,
                material, "Requisição de Compra enviada pelo SAP", "ITEM001", "REQ0001");
            
            return requisicaoDeCompra;
        }

        public static ProcessoDeCotacaoDeMaterial ObtemProcessoDeCotacaoDeMaterialPadrao()
        {
            var requisicaoDeCompra = ObtemRequisicaoDeCompraPadrao();
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra);
            return processoDeCotacao;
        }

        public static Fornecedor ObtemFornecedorPadrao()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 001", "fornecedor0001@empresa.com.br");
            return fornecedor;
        }

        public static Usuario ObtemUsuarioPadrao()
        {
            var usuario = new Usuario("Usuario 0001", "usuario0001", "usuario0001@empresa.com.br",
                                      Enumeradores.Perfil.Comprador);
            return usuario;
        }
    }
}
