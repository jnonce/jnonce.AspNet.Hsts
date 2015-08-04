using System;
using jnonce.AspNet.Hsts;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Builder
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add HTTP Strict Transport Security headers and redirects to the application
        /// </summary>
        /// <param name="app">Application to configure</param>
        /// <param name="options">HSTS options</param>
        public static void AddHsts(this IApplicationBuilder app)
        {
            AddHstsNonSecureRedirect(app);
            app.UseMiddleware<HstsMiddleware>();
        }

        private static void AddHstsNonSecureRedirect(IApplicationBuilder app)
        {
            IHstsRedirectProvider redirectProvider = app.ApplicationServices.GetRequiredService<IHstsRedirectProvider>();
            HstsRedirector redirector = redirectProvider.GetRedirector();
            if (redirector != null)
            {
                app.UseMiddleware<HstsRedirectMiddleware>(redirector);
            }
        }
    }
}
