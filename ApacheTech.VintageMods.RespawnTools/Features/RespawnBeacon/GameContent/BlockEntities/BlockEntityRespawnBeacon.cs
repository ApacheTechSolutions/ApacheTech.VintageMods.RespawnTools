﻿using ApacheTech.Common.Extensions.System;
using ApacheTech.VintageMods.Core.Abstractions.GameContent;
using ApacheTech.VintageMods.Core.Common.StaticHelpers;
using ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.GameContent.Blocks;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

// ReSharper disable ClassNeverInstantiated.Global

namespace ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.GameContent.BlockEntities
{
    /// <summary>
    ///     Represents a specific Respawn Beacon, placed within the game world. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="BlockEntity{BlockRespawnBeacon}" />
    public sealed class BlockEntityRespawnBeacon : BlockEntity<BlockRespawnBeacon>
    {
        /// <summary>
        ///     Gets the spawn position to pass to the player, when they die in range of this beacon, when enabled.
        /// </summary>
        /// <value>The spawn position.</value>
        public FuzzyEntityPos SpawnPosition { get; private set; }

        /// <summary>
        ///     Gets the light colour and brightness.
        /// </summary>
        /// <value>The light levels emitted from this block.</value>
        public byte[] LightHsv => new byte[] { 4, 1, (byte)(Enabled ? 18 : 4) };

        /// <summary>
        ///     Gets or sets a value indicating whether the Respawn Beacon at the given BlockPos is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        ///     Gets or sets the radius at which the Respawn Beacon is active.
        /// </summary>
        /// <value>An <see cref="int"/> value, determining the active radius of the beacon.</value>
        public int Radius { get; set; } = 128;

        /// <summary>
        ///     This method is called right after the block entity was spawned or right after it was loaded from a newly loaded chunk.
        ///     You do have access to the world and its blocks at this point.
        ///     However if this block entity already existed then FromTreeAttributes is called first!
        ///     You should still call the base method to sets the this.api field.
        /// </summary>
        /// <param name="api">The API.</param>
        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            if (Api is not ICoreServerAPI)
            {
                api.Event.RegisterGameTickListener(OnClientGameTick, 50, 50);
                return;
            }
            SpawnPosition = new FuzzyEntityPos(Pos.X + 0.5, Pos.Y + 1, Pos.Z + 0.5)
            {
                UsesLeft = -1
            };
        }

        /// <summary>
        ///     Called when the block was broken in survival mode or through explosions and similar.
        ///     Generally in situations where you probably want to drop the block entity contents, if it has any.
        /// </summary>
        /// <param name="byPlayer">The player that broke the block.</param>
        public override void OnBlockBroken(IPlayer byPlayer = null)
        {
            Enabled = false;
            if (Api is ICoreServerAPI sapi)
            {
                sapi.World.BlockAccessor.RemoveBlockLight(LightHsv, Pos);
            }
            base.OnBlockBroken(byPlayer);
        }

        /// <summary>
        ///     Called when saving the world or when sending the block entity data to the client. When overriding, make sure to still call the base method.
        /// </summary>
        /// <param name="tree">The tree attributes used to resolve this specific block entity.</param>
        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetBool("Enabled", Enabled);
            tree.SetInt("Radius", Radius);
            if (Api.Side.IsClient()) return;
            RespawnBeacon.UpdateBeaconCache(this, true);
        }

        /// <summary>
        ///     Called when loading the world or when receiving block entity from the server.
        ///     When overriding, make sure to still call the base method.
        ///     FromTreeAttributes is always called before Initialize() is called, so the this.api field is not yet set!
        /// </summary>
        /// <param name="tree">The tree attributes used to resolve this specific block entity.</param>
        /// <param name="worldForResolving">
        ///     Use this api if you need to resolve blocks/items. Not suggested for other purposes, as the residing chunk may not be loaded at this point.
        /// </param>
        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
        {
            base.FromTreeAttributes(tree, worldForResolving);
            Enabled = tree.GetBool("Enabled");
            Radius = tree.GetInt("Radius");
            if (worldForResolving.Api.Side.IsClient()) return;
            ApiEx.ServerMain.WorldMap.UpdateLighting(Block.Id, Block.Id, Pos);
        }

        private void OnClientGameTick(float dt)
        {
            if (Block is null) return;
            if (!Enabled) return;
            var particles = OwnerBlock.Particles.With(p =>
            {
                p.UseLighting();
                p.MinPos = new Vec3d(Pos.X, Pos.Y + 1, Pos.Z);
                p.Color = ColorUtil.ColorFromRgba(
                    GameMath.Clamp(RandomEx.RandomValueAround(204, 20), 0, 255),
                    GameMath.Clamp(RandomEx.RandomValueAround(235, 20), 0, 255),
                    GameMath.Clamp(RandomEx.RandomValueAround(178, 20), 0, 255),
                    GameMath.Clamp(RandomEx.RandomValueAround(128, 127), 0, 255));
            });
            ApiEx.ClientMain.SpawnParticles(particles);
        }
    }
}