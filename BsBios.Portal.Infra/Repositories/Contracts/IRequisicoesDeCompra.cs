using System;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IRequisicoesDeCompra:ICompleteRepository<RequisicaoDeCompra>
    {
        RequisicaoDeCompra BuscaPeloId(int id);
        IRequisicoesDeCompra PertencentesAoGrupoDeCompra(string codigoDoGrupoDeCompras);
        IRequisicoesDeCompra SolicitadasApartirDe(DateTime data);
        IRequisicoesDeCompra SolicitadasAte(DateTime data);
        IRequisicoesDeCompra SemProcessoDeCotacao();
    }
}