using Godot;
using System;

namespace Krystal.World;

/// <summary>
/// The root node of the world. Manages all world information
/// </summary>
public partial class WorldManager : WorldEnvironment
{
	private Chunk _chunks;
	private Node3D _chunksNode;
	private PackedScene _blockScene;
	private Node3D _chunkScene;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_blockScene = GD.Load<PackedScene>("World/StaticBlock.tscn");
		_chunksNode = GetNode<Node3D>("Chunks");
		_chunks = new Chunk();
		
		_chunks.Generate();
		
		RenderChunk(_chunks);
		
	}
	
	public void RenderChunk(Chunk chunk)
	{
		for (int x = 0; x < Chunk.ChunkSizeX; x++)
		{
			for (int y = 0; y < Chunk.ChunkSizeY; y++)
			{
				for (int z = 0; z < Chunk.ChunkSizeZ; z++)
				{
					RenderBlock(chunk[x, y, z], x, y, z);
				}
			}
		}
	}
	
	public void RenderBlock(BlockInstance block, int x, int y, int z)
	{
		var newBlock = _blockScene.Instantiate() as StaticBody3D;
			
		if (newBlock == null) 
			throw new Exception("Block instance is null");
			
		var newBlockMesh = newBlock.GetNode<Node3D>("Mesh") as MeshInstance3D;
			
		if (newBlockMesh == null)
			throw new Exception("Mesh instance is null");
			
		newBlock.Position = new Vector3(x, y, z);
		newBlockMesh.MaterialOverride = block.BlockType.BlockTexture;
		GD.Print($"Applying {block.BlockType.BlockTexture.ToString()} to {block.BlockType.Name}");
			
		_chunksNode.AddChild(newBlock);
	}

// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
