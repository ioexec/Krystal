using Godot;
using System;
using Godot.NativeInterop;
using Krystal.World.Exceptions;

namespace Krystal.World;

public class Block
{
	public enum BlockTypeEnum
	{
		Air,
		Grass,
		Stone,
		Dirt
	}

	private BlockTypeEnum _blockType;

	public BlockTypeEnum BlockType
	{
		get => _blockType;
		set
		{
			if (Enum.IsDefined(typeof(BlockTypeEnum), value))
				_blockType = value;
			else
			{
				throw new BlockNotFoundException((int)value);
			}
		}
	}

	public Block(BlockTypeEnum blockTypeEnum)
	{
		BlockType = blockTypeEnum;
	}
}