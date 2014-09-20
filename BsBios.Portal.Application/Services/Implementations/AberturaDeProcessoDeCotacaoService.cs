using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AberturaDeProcessoDeCotacaoService: IAberturaDeProcessoDeCotacaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IGerenciadorUsuario _gerenciadorUsuario;
        private readonly IGeradorDeEmailDeAberturaDeProcessoDeCotacao _geradorDeEmailDeProcessoDeAberturaDeCotacao;
        private readonly IComunicacaoSap _comunicacaoSap;

        public AberturaDeProcessoDeCotacaoService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, 
            IGeradorDeEmailDeAberturaDeProcessoDeCotacao geradorDeEmailDeProcessoDeAberturaDeCotacao, 
            IComunicacaoSap comunicacaoSap, IGerenciadorUsuario gerenciadorUsuario)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _geradorDeEmailDeProcessoDeAberturaDeCotacao = geradorDeEmailDeProcessoDeAberturaDeCotacao;
            _comunicacaoSap = comunicacaoSap;
            _gerenciadorUsuario = gerenciadorUsuario;
        }

        public void Executar(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.Abrir();
                _comunicacaoSap.EfetuarComunicacao(processoDeCotacao);
                _gerenciadorUsuario.CriarSenhaParaUsuariosSemSenha(processoDeCotacao.FornecedoresParticipantes.Select(x => x.Fornecedor.Codigo).ToArray());
                _geradorDeEmailDeProcessoDeAberturaDeCotacao.GerarEmail(processoDeCotacao);
                _processosDeCotacao.Save(processoDeCotacao);
                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();                
                throw;
            }
        }
    }

    //public class AberturaDeProcessoDeCotacaoDeFrete: AberturaDeProcessoDeCotacaoService
    //{
    //    public AberturaDeProcessoDeCotacaoDeFrete(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao, 
    //        IGeradorDeEmailDeAberturaDeProcessoDeCotacao geradorDeEmailDeProcessoDeAberturaDeCotacaoDeFrete,
    //        IComunicacaoSap comunicacaoSap, IGerenciadorUsuario gerenciadorUsuario) :
    //        base(unitOfWork, processosDeCotacao, geradorDeEmailDeProcessoDeAberturaDeCotacaoDeFrete,comunicacaoSap,gerenciadorUsuario)
    //    {
    //    }
    //}

    //public class AberturaDeProcessoDeCotacaoDeMaterial : AberturaDeProcessoDeCotacaoService
    //{
    //    public AberturaDeProcessoDeCotacaoDeMaterial(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao,
    //        IGeradorDeEmailDeAberturaDeProcessoDeCotacao geradorDeEmailDeProcessoDeAberturaDeCotacaoDeMaterial,
    //        IComunicacaoSap comunicacaoSap, IGerenciadorUsuario gerenciadorUsuario) :
    //        base(unitOfWork, processosDeCotacao, geradorDeEmailDeProcessoDeAberturaDeCotacaoDeMaterial,comunicacaoSap,gerenciadorUsuario)
    //    {
    //    }
    //}
    
}
