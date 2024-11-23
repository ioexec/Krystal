using System;
using Godot;

namespace Krystal.World;

public abstract class BlockType
{
    BaseMaterial3D _texture;

    public string Name { get; protected set; }

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

    public virtual void SetDefaults()
    {
        Name = this.GetType().Name;
    }

}