using Krystal.Types;
using Godot;

namespace Krystal.World;

public class Chunk : Array3D<Block>
{
    public const int ChunkSizeX = 16;
    public const int ChunkSizeY = 16;
    public const int ChunkSizeZ = 32;
    
    private bool _isGenerated = false;
    
    public Chunk() : base(ChunkSizeX, ChunkSizeY, ChunkSizeZ)
    { }

    public void Generate()
    {
        GD.Print("Generating chunk");
        
        if (_isGenerated)
            return;
        
        SetAllValues(new Block(Block.BlockTypeEnum.Stone));

        _isGenerated = true;
    }
}