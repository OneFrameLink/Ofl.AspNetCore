using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Builder;

namespace Ofl.AspNetCore.BuilderExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder RemoveHeaders(this IApplicationBuilder applicationBuilder,
            params string[] headers) => applicationBuilder.RemoveHeaders((IEnumerable<string>) headers);

        public static IApplicationBuilder RemoveHeaders(this IApplicationBuilder applicationBuilder, IEnumerable<string> headers)
        {
            // Validate parameters.
            if (applicationBuilder == null) throw new ArgumentNullException(nameof(applicationBuilder));
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            // Materialize the items.
            IReadOnlyCollection<string> hdrs = new ReadOnlyCollection<string>(headers.ToList());

            // SHORTCUT: If there are no headers, then get out, do nothing.
            if (hdrs.Count == 0) return applicationBuilder;

            // Create the middleware and return.
            return applicationBuilder.Use((context, next) => {
                // Cycle through each item.
                foreach (string header in hdrs)
                    // Remove.
                    context.Response.Headers.Remove(header);

                // Return the call to next.
                return next();
            });
        }
    }
}
