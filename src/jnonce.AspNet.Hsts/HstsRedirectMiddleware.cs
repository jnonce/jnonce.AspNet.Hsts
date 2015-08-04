using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;

namespace jnonce.AspNet.Hsts
{
    /// <summary>
    /// Middleware which redirects from non-secure connections to secure ones
    /// </summary>
    public class HstsRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly HstsRedirector _redirectProvider;

        /// <summary>
        /// Creates a new instance of the <see cref="HstsRedirectMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="redirectProvider">Provide redirects for non-secure requests.</param>
        /// <param name="loggerFactory">An <see cref="T:Microsoft.Framework.Logging.ILoggerFactory" /> instance used to create loggers.</param>
        public HstsRedirectMiddleware(RequestDelegate next, HstsRedirector redirectProvider, ILoggerFactory loggerFactory)
        {
            if (redirectProvider == null)
            {
                throw new ArgumentNullException(nameof(redirectProvider));
            }

            this._next = next;
            this._logger = LoggerFactoryExtensions.CreateLogger<HstsRedirectMiddleware>(loggerFactory);
            _redirectProvider = redirectProvider;
        }

        public Task Invoke(HttpContext context)
        {
            if (context.Request.IsHttps)
            {
                return _next(context);
            }

            string targetUri = _redirectProvider(context.Request);
            if (targetUri == null)
            {
                return _next(context);
            }

            _logger.LogInformation("Redirecting from: \"{0}\" to \"{1}\"", context.Request.Path, targetUri);
            context.Response.Redirect(targetUri, permanent: true);

            return Task.FromResult(0);
        }
    }
}
