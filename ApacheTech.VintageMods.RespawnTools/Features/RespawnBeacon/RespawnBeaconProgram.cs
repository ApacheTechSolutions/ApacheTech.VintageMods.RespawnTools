using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.VintageMods.Core.Hosting.DependencyInjection.Registration;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon
{
    /// <summary>
    ///     Configures registration and pre-start conditions for the RespawnBeacon feature.
    /// </summary>
    /// <seealso cref="ClientFeatureRegistrar" />
    public sealed class RespawnBeaconProgram : ClientFeatureRegistrar
    {
        /// <summary>
        ///     Allows a mod to include Singleton, or Transient services to the IOC Container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public override void ConfigureClientModServices(IServiceCollection services)
        {
            services.RegisterTransient<Dialogue.RespawnBeaconDialogue>();
        }
    }
}