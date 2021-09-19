using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Who_Owes_Money.Infrastructure
{
    public class CustomFloatingModelBinder : IModelBinder
    {
        private readonly IModelBinder FallbackModelBinder;
        private string FieldName;
        private string FieldValueAsString;
        private string FieldValueAsNormalString;
        private ValueProviderResult PartValues;

        protected readonly Regex FloatPattern = new Regex(@"^(-?)[0-9]*(?:[.,][0-9]*)?$", RegexOptions.Compiled);
        protected readonly Regex FloatSeparator = new Regex(@"[.,]", RegexOptions.Compiled);

        public CustomFloatingModelBinder(IModelBinder fallBackModelBinder)
        {
            FallbackModelBinder = fallBackModelBinder;
        }

        public Task BindModelAsync(ModelBindingContext modelBindingContext)
        {
            if (modelBindingContext == null)
                throw new ArgumentNullException(nameof(modelBindingContext));

            FieldName = modelBindingContext.FieldName;

            PartValues = modelBindingContext.ValueProvider.GetValue(FieldName);

            if (PartValues == ValueProviderResult.None)
                return FallbackModelBinder.BindModelAsync(modelBindingContext);

            FieldValueAsString = FieldValueAsNormalString = PartValues.FirstValue;

            // ".5" -> "0.50"
            // "5." -> "05.0"
            if (FloatSeparator.IsMatch(FieldValueAsNormalString))
                FieldValueAsNormalString = "0" + FieldValueAsNormalString + "0";

            FieldValueAsNormalString = FloatSeparator.Replace(FieldValueAsNormalString, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            if (!FloatPattern.IsMatch(FieldValueAsNormalString))
                return FallbackModelBinder.BindModelAsync(modelBindingContext);

            if (modelBindingContext.ModelMetadata.ModelType == typeof(double))
                modelBindingContext.Result = ModelBindingResult.Success(GetDoubleFromString);
            else if (modelBindingContext.ModelMetadata.ModelType == typeof(float))
                modelBindingContext.Result = ModelBindingResult.Success(GetFloatFromString);
            else if (modelBindingContext.ModelMetadata.ModelType == typeof(decimal))
                modelBindingContext.Result = ModelBindingResult.Success(GetDecimalFromString);
            else
                return FallbackModelBinder.BindModelAsync(modelBindingContext);

            return Task.CompletedTask;
        }

        public double GetDoubleFromString
        {
            get
            {
                if (!double.TryParse(FieldValueAsNormalString, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out double ret_val))
                    return 0;

                return ret_val;
            }
        }

        public decimal GetDecimalFromString
        {
            get
            {
                if (!decimal.TryParse(FieldValueAsNormalString, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal ret_val))
                    return 0;

                return ret_val;
            }
        }

        public float GetFloatFromString
        {
            get
            {
                if (!float.TryParse(FieldValueAsNormalString, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out float ret_val))
                    return 0;

                return ret_val;
            }
        }
    }
}
