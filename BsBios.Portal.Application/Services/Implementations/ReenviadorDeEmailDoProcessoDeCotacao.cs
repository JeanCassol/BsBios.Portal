using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Services.Contracts;
using System.Linq;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ReenviadorDeEmailDoProcessoDeCotacao : IReenviadorDeEmailDoProcessoDeCotacao
    {
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IGeradorDeEmailDeAberturaDeProcessoDeCotacao _geradorDeEmail;

        public ReenviadorDeEmailDoProcessoDeCotacao(IProcessosDeCotacao processosDeCotacao, 
            IGeradorDeEmailDeAberturaDeProcessoDeCotacao geradorDeEmail)
        {
            _processosDeCotacao = processosDeCotacao;
            _geradorDeEmail = geradorDeEmail;
        }

        public void ReenviarEmailDeAbertura(int idProcessoCotacao, int idFornecedorParticipante)
        {
            ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
            FornecedorParticipante fornecedorParticipante = processoDeCotacao.FornecedoresParticipantes.First(x => x.Id == idProcessoCotacao);
            _geradorDeEmail.GerarEmail(fornecedorParticipante);
        }
    }
}