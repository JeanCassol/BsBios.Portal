using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IConhecimentosDeTransporte: ICompleteRepository<ConhecimentoDeTransporte>
    {
        IConhecimentosDeTransporte ComChaveEletronica(string chave);
        IConhecimentosDeTransporte IncluirNotasFiscais();
    }
}