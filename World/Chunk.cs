using System.Collections.Concurrent;
using Krystal.Types;
using Godot;

namespace Krystal.World;

/// <summary>
/// Represents a chunk in world space. Including its associated metadata and block data.
/// </summary>
public class Chunk : Array3D<BlockInstance>, IGenerated
{
    public const int ChunkSizeX = 16;
    public const int ChunkSizeY = 32;
    public const int ChunkSizeZ = 16;
    
    private bool _isGenerated = false;
    
    public Chunk() : base(ChunkSizeX, ChunkSizeY, ChunkSizeZ)
    { }

    /// <summary>
    /// Generates the block data and metadata for this chunk
    /// </summary>
    public void Generate()
    {
        GD.Print("Generating chunk");
        
        if (_isGenerated)
            return;

        var grassblock = new BlockInstance();
        grassblock.BlockType = ContentManager.GetBlockTypeByName("Grass");
        
        SetAllValues(grassblock);

        for (int x = 0; x < ChunkSizeX; x++)
        {
            for (int y = 0; y < ChunkSizeY; y++)
            {
                for (int z = 0; z < ChunkSizeZ; z++)
                {
                    if (y < ChunkSizeY - 4)
                    {
                        var new_block = new BlockInstance();
                        new_block.BlockType = ContentManager.GetBlockTypeByName("Stone");
                        this[x, y, z] = new_block;

                    }
                    else
                    {
                        var new_block = new BlockInstance();
                        new_block.BlockType = ContentManager.GetBlockTypeByName("Grass");
                        this[x, y, z] = new_block;
                    }
                        
                }
            }
        }

        _isGenerated = true;
    }
}