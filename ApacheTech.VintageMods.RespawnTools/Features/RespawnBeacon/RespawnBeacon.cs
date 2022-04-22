using System.Collections.Generic;
using System.Linq;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Extensions.Game;
using ApacheTech.VintageMods.Core.Services;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.GameContent.BlockEntities;
using ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.GameContent.Blocks;
using ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.Model;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon
{
    /// <summary>
    ///     Mod Entry-point for the RespawnBeacon feature.
    /// </summary>
    /// <seealso cref="UniversalModSystem" />
    public sealed class RespawnBeacon : UniversalModSystem
    {
        public static List<EnabledBeacon> EnabledBeacons { get; private set; }

        /// <summary>
        ///     Updates a beacon, within the beacon cache.
        /// </summary>
        /// <param name="beacon">The beacon.</param>
        /// <param name="addEnabled">if set to <c>true</c> adds the beacon to the cache, if the beacon is enabled.</param>
        public static void UpdateBeaconCache(BlockEntityRespawnBeacon beacon, bool addEnabled)
        {
            if (beacon.Pos is null) return;
            if (EnabledBeacons is null) return;
            EnabledBeacons.RemoveAll(p => p.Position == null);
            EnabledBeacons.RemoveAll(p => p.Position == beacon.Pos);
            if (beacon.Enabled && addEnabled) EnabledBeacons.Add(EnabledBeacon.FromBlockEntity(beacon));
            ModServices.FileSystem.GetJsonFile("beacon-cache-server.json").SaveFrom(EnabledBeacons);
        }

        /// <summary>
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">The API.</param>
        public override void Start(ICoreAPI api)
        {
            api.Network
                .RegisterChannel("RespawnBeacon")
                .RegisterMessageType<RespawnBeaconPacket>();
            api.RegisterBlock<BlockRespawnBeacon>();
            api.RegisterBlockEntity<BlockEntityRespawnBeacon>();
        }

        public override void StartPre(ICoreAPI api)
        {
            if (api.Side.IsClient()) return;
            ModServices.FileSystem.RegisterFile("beacon-cache-server.json", FileScope.World);
        }

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreServerAPI in Start()
        /// </summary>
        /// <param name="api">The API.</param>
        public override void StartServerSide(ICoreServerAPI api)
        {
            EnabledBeacons = ModServices.FileSystem
                .GetJsonFile("beacon-cache-server.json")
                .ParseAsMany<EnabledBeacon>().ToList();
        }

        /// <summary>
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers
        /// </summary>
        public override void Dispose()
        {
            EnabledBeacons?.Clear();
            EnabledBeacons = null;
        }
    }
}
