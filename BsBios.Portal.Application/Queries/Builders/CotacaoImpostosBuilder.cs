using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Builders
{

    public class CotacaoImpostosBuilder : Builder<CotacaoItem, CotacaoImpostosVm>
    {
        public override CotacaoImpostosVm BuildSingle(CotacaoItem cotacaoItem)
        {
            var vm = new CotacaoImpostosVm();
            Imposto imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.Icms);
            if (imposto != null)
            {
                vm.IcmsAliquota = imposto.Aliquota;
                vm.IcmsValor = imposto.Valor;
            }

            imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.IcmsSubstituicao);
            if (imposto != null)
            {
                vm.IcmsStAliquota = imposto.Aliquota;
                vm.IcmsStValor = imposto.Valor;
            }

            imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.Ipi);
            if (imposto != null)
            {
                vm.IpiAliquota = imposto.Aliquota;
                vm.IpiValor = imposto.Valor;
            }

            imposto = cotacaoItem.Imposto(Enumeradores.TipoDeImposto.PisCofins);
            if (imposto != null)
            {
                vm.PisCofinsAliquota = imposto.Aliquota;
                vm.PisCofinsValor = imposto.Valor;
            }

            //imposto = cotacao.Imposto(Enumeradores.TipoDeImposto.Pis);
            //if (imposto != null)
            //{
            //    vm.PisAliquota = imposto.Aliquota;
            //    vm.PisValor = imposto.Valor;
            //}


            //imposto = cotacao.Imposto(Enumeradores.TipoDeImposto.Cofins);
            //if (imposto != null)
            //{
            //    vm.CofinsAliquota = imposto.Aliquota;
            //    vm.CofinsValor = imposto.Valor;
            //}

            return vm;

        }

    }
}
