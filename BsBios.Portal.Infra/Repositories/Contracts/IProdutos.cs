using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProdutos : ICompleteRepository<Produto>
    {
        Produto BuscaPeloCodigo(string codigoSap);
        IProdutos FiltraPorListaDeCodigos(string[] codigos);
        IProdutos DescricaoContendo(string filtroDescricao);
        IProdutos CodigoContendo(string codigo);
        IProdutos TipoContendo(string tipo);
    }
}