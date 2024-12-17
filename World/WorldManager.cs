using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Krystal.Graphics;
using Krystal.Graphics.World;

namespace Krystal.World;

/// <summary>
/// The root node of the world. Manages all world information and creation of meshes
/// </summary>
public partial class WorldManager : WorldEnvironment
{
	private Chunk _chunks;
	private Node3D _chunksNode;
	private PackedScene _blockScene;
	private Node3D _chunkScene;
	private ChunkMeshBuilder _chunkBuilder;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_blockScene = GD.Load<PackedScene>("World/StaticBlock.tscn");
		_chunksNode = GetNode<Node3D>("Chunks");
		_chunks = new Chunk();
		
		_chunkBuilder = new ChunkMeshBuilder();
		
		_chunks.Generate();
		
		List<Quad> quads = new List<Quad>();
		var q = new Quad();
		q.A = new Vector3(-1, 1, 0);
		q.B = new Vector3(1, 1, 0);
		q.C = new Vector3(-1, -1, 0);
		q.D = new Vector3(1, -1, 0);
		quads.Add(q);
		
		List<Quad> quads2 = new List<Quad>();
		var q2 = new Quad();
		q2.A = new Vector3(1, 1, 0);
		q2.B = new Vector3(2, 1, 0);
		q2.C = new Vector3(1, -1, 0);
		q2.D = new Vector3(2, -1, 0);
		quads.Add(q2);
		
		Quad.Merge(quads);

	}

	public void RenderChunk(Chunk chunk)
	{
		
		MeshInstance3D chunkMesh = new MeshInstance3D();
		
		// MeshInstance3D.
		
	}
	
	// TODO: also check the chunks next to them and cull accordingly
	private void BuildChunkMesh(Chunk chunk)
	{
		var chunkMesh = new ArrayMesh();
		var verts = new List<Vector3>();
		var uvs = new List<Vector2>();
		var normals = new List<Vector3>();
		var indices = new List<int>();

		for (int x = 0; x < chunk.SizeX - 1; x++)
		{
			for (int y = 0; y < chunk.SizeY - 1; y++)
			{
				for (int z = 0; z < chunk.SizeZ - 1; z++)
				{
					var block = chunk[x, y, z];
                    
					// If the block on each side is transparent, then make a face on that side
					if (chunk[x + 1, y, z].BlockType.Transparent)
					{
						Quad quad = new Quad();
						
                        c	
					}
				}
			}
		}
	}

// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
