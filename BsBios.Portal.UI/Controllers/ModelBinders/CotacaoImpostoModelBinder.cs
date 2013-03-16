using System;
using System.Web.Mvc;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers.ModelBinders
{
    public class CotacaoImpostoModelBinder:IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var form = controllerContext.RequestContext.HttpContext.Request.Form;
            return new CotacaoImpostosVm
                {
                    IcmsAliquota = Convert.ToDecimal(form["IcmsAliquota"])  ,
                    IcmsValor = Convert.ToDecimal(form["IcmsValor"]),
                    IcmsStAliquota = Convert.ToDecimal(form["IcmsStAliquota"]),
                    IcmsStValor = Convert.ToDecimal(form["IcmsStValor"]),
                    IpiAliquota = Convert.ToDecimal(form["IpiAliquota"]),
                    IpiValor = Convert.ToDecimal(form["IpiValor"]),
                    PisCofinsAliquota = Convert.ToDecimal(form["PisCofinsAliquota"])
                };

        }
    }
}