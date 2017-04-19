using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace Ofl.AspNetCore.Mvc.ModelBinding
{
    public static class ValueProviderExtensions
    {
        public static string GetSingleOrDefaultValue(this IValueProvider valueProvider, string key)
        {
            // Validate parameters.
            if (valueProvider == null) throw new ArgumentNullException(nameof(valueProvider));

            // Return single or default.
            return valueProvider.GetValue(key).Values.SingleOrDefault();
        }
    }
}
