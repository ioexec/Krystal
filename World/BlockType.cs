using System;
using Godot;

namespace Krystal.World;

/// <summary>
/// Abstract representation of a block and all its metadata and custom behaviour
/// </summary>
public abstract class BlockType
{
    BaseMaterial3D _texture;

    /// <summary>
    /// Internal name of this block
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// The block's texture
    /// </summary>
    /// <exception cref="Exception"></exception>
    public BaseMaterial3D BlockTexture
    {
        get => _texture;
        set
        {
            if (_texture == null)
                _texture = value;
            else
                throw new Exception("Cannot change texture while it is already set");
        }
    }
    
    /// <summary>
    /// Called during registration. Sets all the default attributes of the block.
    /// </summary>
    public virtual void SetDefaults()
    {
        Name = this.GetType().Name;
    }

}