using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;
using Microsoft.Framework.OptionsModel;

namespace jnonce.AspNet.Hsts
{
    /// <summary>
    /// Middleware which sets the Strict-Transport-Security header on secure connections
    /// </summary>
    public class HstsMiddleware
    {
        private const string StsHeader = "Strict-Transport-Security";

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly string _headerValue;

        /// <summary>
        /// Creates a new instance of the StaticFileMiddleware.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="options">The configuration options.</param>
        /// <param name="loggerFactory">An <see cref="T:Microsoft.Framework.Logging.ILoggerFactory" /> instance used to create loggers.</param>
        public HstsMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IOptions<HstsOptions> options)
        {
            this._next = next;
            this._headerValue = GetHeaderValue(options.Options);
            this._logger = LoggerFactoryExtensions.CreateLogger<HstsMiddleware>(loggerFactory);
        }

        /// <summary>
        /// Execute the request
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
            {
                //context.Response.OnStarting(() => WriteHeadersAsync((HttpContext)context));
                await WriteHeadersAsync(context);
            }
            await _next(context);
        }

        private Task WriteHeadersAsync(HttpContext context)
        {
            _logger.LogInformation("Adding header for secure request");
            try
            {
                IHeaderDictionary headers = context.Response.Headers;
                headers.Set(StsHeader, _headerValue);
            }
            catch (Exception error)
            {
                _logger.LogError("Failed to set header", error);
            }
            return Task.FromResult(0);
        }

        private static string GetHeaderValue(HstsOptions options)
        {
            List<string> parts = new List<string>();

            TimeSpan maxAge = options.MaxAge;
            if (maxAge < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(options.MaxAge));
            }

            parts.Add($"max-age={maxAge.TotalSeconds : 0}");
            if (options.IncludeSubDomains)
            {
                parts.Add("includeSubDomains");
            }
            if (options.Preload)
            {
                parts.Add("preload");
            }
            return String.Join("; ", parts);
        }
    }
}
