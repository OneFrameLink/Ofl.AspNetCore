using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Primitives;

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

        public static IApplicationBuilder AddHeaders(this IApplicationBuilder applicationBuilder,
            params KeyValuePair<string, StringValues>[] headers) => applicationBuilder.AddHeaders((IEnumerable<KeyValuePair<string, StringValues>>) headers);

        public static IApplicationBuilder AddHeaders(this IApplicationBuilder applicationBuilder,
            IEnumerable<KeyValuePair<string, StringValues>> headers)
        {
            // Validate parameters.
            if (applicationBuilder == null) throw new ArgumentNullException(nameof(applicationBuilder));
            if (headers == null) throw new ArgumentNullException(nameof(headers));

            // Materialize.
            IReadOnlyCollection<KeyValuePair<string, StringValues>> hdrs = new ReadOnlyCollection<KeyValuePair<string, StringValues>>(headers.ToList());

            // If there are no headers, get out, do nothing.
            if (hdrs.Count == 0) return applicationBuilder;

            // Create middleware and return.
            return applicationBuilder.Use((context, next) => {
                // Cycle through each item.
                foreach (KeyValuePair<string, StringValues> header in hdrs)
                    // Add.
                    context.Response.Headers.Add(header);

                // Return the call to next.
                return next();
            });
        }
    }
}
