using ApacheTech.VintageMods.Core.Annotation.Attributes;
using Vintagestory.API.Common;

// ReSharper disable StringLiteralTypo

[assembly: ModDependency("game", "1.16.4")]
[assembly: ModDependency("survival", "1.16.4")]

[assembly:ModInfo(
    "Respawn Tools",
    "respawntools",
    Description = "Adds a `Respawn Beacon`, that when Enabled, will cause any players in range, to respawn at the beacon when they die, rather than at their default spawn position.",
    Side = "Universal",
    Version = "1.0.0",
    NetworkVersion = "1.0.0",
    IconPath = "modicon.png",
    Website = "https://apachetech.co.uk",
    Contributors = new[] { "ApacheTech Solutions" },
    Authors = new []{ "ApacheTech Solutions" })]

[assembly: VintageModInfo(
    ModId = "respawntools",
    ModName = "Respawn Tools",
    RootDirectoryName = "RespawnTools",
    NetworkVersion = "1.0.0",
    Version = "1.0.0", 
    Side = EnumAppSide.Universal)]