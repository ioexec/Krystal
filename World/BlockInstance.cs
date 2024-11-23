using System;
using System.Threading.Tasks;
using Godot;

namespace Krystal.World;

/// <summary>
/// Represents the instance of a <c>BlockType</c> in a <c>Chunk</c>.
/// </summary>
public struct BlockInstance
{
    /// <summary>
    /// A reference type to the registered <c>BlockType</c> this instance represents.
    /// </summary>
    private BlockType _blockType;
    
    /// <summary>
    /// A reference type to the registered <c>BlockType</c> this instance represents.
    /// </summary>
    public BlockType BlockType
    {
        get => _blockType;
        set
        {
            if (_blockType == null)
            {
                if (value.GetType().IsSubclassOf(typeof(BlockType)))
                {
                    GD.Print($"Instance created of {value.GetType().Name} ");
                    _blockType = value;
                }
                else
                {
                    throw new Exception("Block type is not a subclass of BlockType");
                }
            }
            else
            {
                throw new Exception("Block type is already set");
            }
        }
    } 
}