using ApacheTech.VintageMods.Core.Hosting;
using ApacheTech.VintageMods.Core.Services;
using Vintagestory.API.Common;

// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.RespawnTools
{
    /// <summary>
    ///     Entry-point for the mod. This class will configure and build the IOC Container, and Service list for the rest of the mod.
    ///     
    ///     Registrations performed within this class should be global scope; by convention, features should aim to be as stand-alone as they can be.
    /// </summary>
    /// <remarks>
    ///     Only one derived instance of this class should be added to any single mod within
    ///     the VintageMods domain. This class will enable Dependency Injection, and add all
    ///     of the domain services. Derived instances should only have minimal functionality, 
    ///     instantiating, and adding Application specific services to the IOC Container.
    /// </remarks>
    /// <seealso cref="ModHost" />
    public sealed class Program : ModHost
    {
        /// <summary>
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client.
        ///     Cast to ICoreServerAPI or ICoreClientAPI to access side specific features.
        /// </param>
        public override void Start(ICoreAPI api)
        {
            ModServices.Harmony.UseHarmony();
        }
    }
}