using System;
using System.Globalization;
using System.Web.Mvc;

namespace BsBios.Portal.UI.Controllers.ModelBinders
{
    public class IntegerModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = int.Parse(valueResult.AttemptedValue, NumberStyles.AllowThousands, CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}