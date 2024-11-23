using System;
using System.Threading.Tasks;
using Godot;

namespace Krystal.World;

public struct BlockInstance
{
    private BlockType _blockType;
    
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