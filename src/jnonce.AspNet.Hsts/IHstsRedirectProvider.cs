using Microsoft.AspNet.Http;

namespace jnonce.AspNet.Hsts
{
    /// <summary>
    /// Provides Hsts redirects for non-secure connections
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public delegate string HstsRedirector(HttpRequest request);

    /// <summary>
    /// Provides HSTS redirect behavior to middleware
    /// </summary>
    public interface IHstsRedirectProvider
    {
        /// <summary>
        /// Gets the redirect method to use when non-secure requests are detected.
        /// </summary>
        /// <returns>Redirect method, or null for no-redirection</returns>
        HstsRedirector GetRedirector();
    }

}
