using System;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Http;
using Microsoft.Framework.OptionsModel;

namespace jnonce.AspNet.Hsts
{

    /// <summary>
    /// Hsts redirection behavior
    /// </summary>
    public class HstsRedirectProvider : IHstsRedirectProvider
    {
        private static readonly Regex RemovePort = new Regex(@"\:.*", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private readonly IOptions<HstsRedirectOptions> options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public HstsRedirectProvider(IOptions<HstsRedirectOptions> options)
        {
            this.options = options;
        }

        /// <summary>
        /// Gets the redirect method used for non-secure connections
        /// </summary>
        /// <returns></returns>
        public HstsRedirector GetRedirector()
        {
            var opts = this.options.Options;
            if (!opts.RedirectNonSecureConnections)
            {
                return null;
            }
            else if (opts.RedirectToAuthority != null)
            {
                return GetRedirctUsingHost(opts.RedirectToAuthority);
            }
            else
            {
                return GetStandardRedirectUri;
            }
        }

        /// <summary>
        /// Get a redirect uri using standard options
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string GetStandardRedirectUri(HttpRequest request)
        {
            var uri = new UriBuilder();
            uri.Scheme = "https";
            uri.Host = RemovePort.Replace(request.Host.Value, String.Empty);
            uri.Path = request.Path.Value;
            uri.Query = request.QueryString.Value;

            return uri.ToString();
        }

        /// <summary>
        /// Gets a redirect provider which using a single, common host
        /// </summary>
        /// <param name="host">Host name to use for all redirects</param>
        /// <returns>Redirect provider method</returns>
        private static HstsRedirector GetRedirctUsingHost(string host)
        {
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            return request =>
            {
                var uri = new UriBuilder();
                uri.Scheme = "https";
                uri.Host = host;
                uri.Path = request.Path.Value;
                uri.Query = request.QueryString.Value;

                return uri.ToString();
            };
        }
    }
}
