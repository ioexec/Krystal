using System.Runtime.CompilerServices;
using Godot;
using Krystal.World;

namespace Krystal.World.Blocks;

public class Grass : BlockType
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        this.BlockTexture = ContentManager.GetTexture("Grass");
    }
}