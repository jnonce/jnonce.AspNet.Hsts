using System;

namespace jnonce.AspNet.Hsts
{
    /// <summary>
    /// Control Hsts settings
    /// </summary>
    public class HstsOptions
    {
        public static TimeSpan DefaultMaxAge => TimeSpan.FromDays(30 * 6);

        /// <summary>
        /// Gets or sets the duration for which the UA regards the host as a known HSTS host.
        /// </summary>
        public TimeSpan MaxAge { get; set; } = DefaultMaxAge;

        /// <summary>
        /// Gets or sets a flag signalling that the HSTS Policy applies
        /// subdomains of the host's domain name.
        /// </summary>
        public bool IncludeSubDomains { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating that the HSTS directive is in a preload table
        /// </summary>
        public bool Preload { get; set; }
    }

    /// <summary>
    /// Control HSTS non-secure redirect behavior
    /// </summary>
    public class HstsRedirectOptions
    {
        /// <summary>
        /// Gets or sets a flag indicating whether the HSTS middleware will redirect requests from non-secure connections
        /// </summary>
        public bool RedirectNonSecureConnections { get; set; } = true;

        /// <summary>
        /// Gets or sets the host and port where redirected requests are sent.
        /// </summary>
        public string RedirectToAuthority { get; set; }
    }
}
