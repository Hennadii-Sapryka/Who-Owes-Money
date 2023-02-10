using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;


namespace Who_Owes_Money.Infrastructure
{
    class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            ILoggerFactory loggerFactory = context.Services.GetRequiredService<ILoggerFactory>();
            IModelBinder binder = new CustomDecimalModelBinder(new SimpleTypeModelBinder(typeof(decimal), loggerFactory));
            return context.Metadata.ModelType == typeof(decimal) ? binder : null ;
        }
    }

    public class CustomDecimalModelBinder : IModelBinder
    {
        private readonly IModelBinder _binder;
        public CustomDecimalModelBinder(IModelBinder binder)
        {
            this._binder = binder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var decimalValues = bindingContext.ValueProvider.GetValue("Price").FirstValue;
            var result = Convert.ToDecimal(decimalValues, new CultureInfo("ua-UA"));
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
