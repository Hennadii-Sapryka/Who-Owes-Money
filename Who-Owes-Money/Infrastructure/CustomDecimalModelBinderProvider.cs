using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace Who_Owes_Money.Infrastructure
{
    public class CustomDecimalModelBinderProvider : IModelBinderProvider
    {
        private IModelBinder binder;

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            ILoggerFactory logger = context.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;

            if (context.Metadata.ModelType == typeof(double))
                binder = new CustomFloatingModelBinder(new SimpleTypeModelBinder(typeof(double), logger));

            else if (context.Metadata.ModelType == typeof(float))
                binder = new CustomFloatingModelBinder(new SimpleTypeModelBinder(typeof(float), logger));

            else if (context.Metadata.ModelType == typeof(decimal))
                binder = new CustomFloatingModelBinder(new SimpleTypeModelBinder(typeof(decimal), logger));

            else
                return null;

            return binder;
        }
    }
}
