using System;
using jnonce.AspNet.Hsts;

namespace Microsoft.Framework.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add HTTP Strict Transport Security services to the application
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void AddHsts(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IHstsRedirectProvider, HstsRedirectProvider>();
        }

        /// <summary>
        /// Configure how HTTP Strict Transport Security headers are set
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="setupAction"></param>
        /// <param name="order"></param>
        public static void ConfigureHsts(this IServiceCollection serviceCollection, Action<HstsOptions> setupAction, int order = 0)
        {
            serviceCollection.Configure(setupAction, order);
        }

        /// <summary>
        /// Configure how requests from non-secure connections under HTTP Strict Transport Security are handled.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="setupAction"></param>
        /// <param name="order"></param>
        public static void ConfigureHstsRedirect(this IServiceCollection serviceCollection, Action<HstsRedirectOptions> setupAction, int order = 0)
        {
            serviceCollection.Configure(setupAction, order);
        }
    }
}
