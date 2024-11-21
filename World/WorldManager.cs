using Godot;
using System;

namespace Krystal.World;

public partial class WorldManager : WorldEnvironment
{
	private Chunk _chunks;
	private Node3D _chunksNode;
	private PackedScene _blockScene;
	private Node3D _chunkScene;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_blockScene = GD.Load<PackedScene>("World/Block.tscn");
		_chunksNode = GetNode<Node3D>("Chunks");
		_chunks = new Chunk();
		
		CreateChunkObject(_chunks);
		
	}

	public void CreateChunkObject(Chunk chunk)
	{
		for (int x = 0; x < Chunk.ChunkSizeX; x++)
		{
			for (int y = 0; y < Chunk.ChunkSizeY; y++)
			{
				for (int z = 0; z < Chunk.ChunkSizeZ; z++)
				{
					GD.Print("Instantiating block");
					var newBlock = _blockScene.Instantiate() as StaticBody3D;
					newBlock.Position = new Vector3(x, y, z);
					_chunksNode.AddChild(newBlock);
				}
			}
		}
	}

// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
