using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public abstract class AtualizadorDeImpostosDaCotacao
    {
        public void AtualizarImpostos(CotacaoItem cotacao, CotacaoImpostosVm cotacaoInformarVm)
        {
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, cotacaoInformarVm.IcmsAliquota.Value);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, cotacaoInformarVm.IcmsStAliquota.Value);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Ipi, cotacaoInformarVm.IpiAliquota.Value);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, cotacaoInformarVm.PisCofinsAliquota.Value);

            //cotacao.InformarImposto(Enumeradores.TipoDeImposto.Pis, cotacaoInformarVm.PisAliquota, cotacaoInformarVm.PisValor);
            //cotacao.InformarImposto(Enumeradores.TipoDeImposto.Cofins, cotacaoInformarVm.CofinsAliquota, cotacaoInformarVm.CofinsValor);

        }
    }
}
