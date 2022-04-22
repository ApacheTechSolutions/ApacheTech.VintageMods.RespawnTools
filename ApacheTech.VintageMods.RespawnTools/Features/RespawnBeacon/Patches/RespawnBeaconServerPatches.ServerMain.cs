using System.Linq;
using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.GameContent.BlockEntities;
using HarmonyLib;
using Vintagestory.API.Common.Entities;
using Vintagestory.Server;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.Patches
{
    public sealed partial class RespawnBeaconServerPatches
    {
        /// <summary>
        ///     Applies a <see cref="HarmonyPrefix"/> patch to the "GetSpawnPosition" method of the <see cref="ServerMain"/> class.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="ServerMain"/> this patch has been applied to.</param>
        /// <param name="__result">The <see cref="FuzzyEntityPos"/> value that would be returned from the original method.</param>
        /// <param name="playerUID">The player's UID.</param>
        /// <returns>
        ///     Returns the spawn position of the nearest enabled Respawn Beacon in range, or passes back to the original method, if no Beacons meet the criteria.
        /// </returns>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ServerMain), "GetSpawnPosition")]
        public static bool Patch_ServerMain_GetSpawnPosition_Prefix(ServerMain __instance, ref FuzzyEntityPos __result, string playerUID)
        {
            var player = __instance.PlayerByUid(playerUID);
            if (player.Entity.Alive) return true;
            var pos = player.Entity.Pos;

            var cache = RespawnBeacon
                .EnabledBeacons
                .Where(p => p.Position is not null)
                .Where(p => p.Position.InRangeHorizontally((int)pos.X, (int)pos.Z, p.Radius))
                .OrderBy(p => p.Position.DistanceTo(pos.AsBlockPos));

            foreach (var beaconInfo in cache)
            {
                if (__instance.BlockAccessor.GetBlockEntity(beaconInfo.Position) is not BlockEntityRespawnBeacon beacon) continue;
                __result = beacon.SpawnPosition.With(p =>
                {
                    p.Yaw = pos.Yaw;
                    p.Pitch = pos.Pitch;
                    p.Roll = pos.Roll;
                    p.Stance = pos.Stance;
                });
                return false;
            }
            return true;
        }
    }
}
