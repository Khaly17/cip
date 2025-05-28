using System;
using System.Globalization;
using System.Web.Mvc;

namespace Gefco.CipQuai.Web
{
    public class ModelBinderConfig
    {
        public static void RegisterModelBinders(System.Web.Mvc.ModelBinderDictionary modelBinders)
        {
            modelBinders.Add(typeof(DateTime), new CultureAwareDateTimeModelBinder());
        }
    }
    public class CultureAwareDateTimeModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                actualValue = Convert.ToDateTime(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }
            catch (Exception e1)
            {
                actualValue = DateTime.Now.Date;
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}