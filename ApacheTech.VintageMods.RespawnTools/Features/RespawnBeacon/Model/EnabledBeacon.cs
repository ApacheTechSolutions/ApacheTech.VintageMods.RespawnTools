using System;
using ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.GameContent.BlockEntities;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;

// ReSharper disable MemberCanBePrivate.Global

namespace ApacheTech.VintageMods.RespawnTools.Features.RespawnBeacon.Model
{
    /// <summary>
    ///     Represents an enabled beacon, within the game world.
    /// </summary>
    /// <seealso cref="IEquatable{EnabledBeacon}" />
    [JsonObject]
    public class EnabledBeacon : IEquatable<EnabledBeacon>
    {
        /// <summary>
        /// 	Initialises a new instance of the <see cref="EnabledBeacon"/> class.
        /// </summary>
        public EnabledBeacon() { /* Reserved by JSON Deserialiser. */ }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="EnabledBeacon"/> class.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="radius">The radius.</param>
        private EnabledBeacon(BlockPos pos, int radius)
        {
            Position = pos;
            Radius = radius;
        }

        /// <summary>
        /// 	Initialises a new instance of the <see cref="EnabledBeacon"/> class.
        /// </summary>
        /// <param name="beacon">The block entity in the game world to populate values from.</param>
        public static EnabledBeacon FromBlockEntity(BlockEntityRespawnBeacon beacon)
        {
            return new EnabledBeacon(beacon.Pos, beacon.Radius);
        }

        /// <summary>
        ///     Gets a value indicating whether the Respawn Beacon at the given BlockPos is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public BlockPos Position { get; }

        /// <summary>
        ///     Gets the radius at which the Respawn Beacon is active.
        /// </summary>
        /// <value>An <see cref="int"/> value, determining the active radius of the beacon.</value>
        public int Radius { get; }

        /// <summary>
        ///     Indicates whether the current object is equal to another object, based on the block position alone.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>true if the current object's block position is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(BlockPos other)
        {
            return Equals(Position, other);
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(EnabledBeacon other)
        {
            return Equals(other?.Position) && Radius == other?.Radius;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is EnabledBeacon other && Equals(other);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Position != null ? Position.GetHashCode() : 0) * 397) ^ Radius;
            }
        }
    }
}