using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using System.Reflection;

using Krystal.World;
using EditorFileSystemImportFormatSupportQuery = Godot.EditorFileSystemImportFormatSupportQuery;

namespace Krystal;


// Find all images.
// For each image
    // Create a ImageTexture
    // Copy StandardBlockMaterial
    // Change the copy's Albedo->Texture to the ImageTexture
    // Place the Material in the associative array with the key as its name without the file extension

public static class ContentManager
{
    private static Dictionary<string, StandardMaterial3D> _textureRegistry;
    private static Dictionary<string, BlockType> _blockRegistry;

    private static StandardMaterial3D _standardBlockMaterial;
    
    // private BlockRegistry _blockRegistry;
    
    static ContentManager()
    {
        _textureRegistry = new Dictionary<string, StandardMaterial3D>();
        _standardBlockMaterial = GD.Load<StandardMaterial3D>("res://Resources/Materials/StandardBlockMaterial.tres");
        LoadTextures("Resources/Textures");

        _blockRegistry = new Dictionary<string, BlockType>();
        LoadBlocks();
        return;
    }
    
    public static bool DoesBlockTypeExistByName(string name)
    {
        return _blockRegistry.ContainsKey(name);
    }
    
    public static void LoadBlocks()
    {
        // Get all classes that inherit BlockType (using reflection trickery!!!)
        var blockTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
            type.Namespace == "Krystal.World.Blocks"
            && !type.IsAbstract
            && !type.IsInterface
            && type.IsSubclassOf(typeof(BlockType)));

        foreach (var type in blockTypes)
        {
            GD.Print($"Instantiating block type {type.Name}");
            var newBlock = Activator.CreateInstance(type) as BlockType;
            
            if (newBlock == null)
                throw new Exception($"Failed to instantiate block type \"{type.Name}\"");
            
            newBlock.SetDefaults();
            
            if (newBlock.BlockTexture == null)
            {
                // Try to find its texture based off of class name alone
                GD.Print($"Finding texture for block \"{newBlock.Name}\"");

                if (DoesTextureExist(type.Name))
                    newBlock.BlockTexture = GetTexture(newBlock.Name);
                else
                {
                    GD.Print($"Could not find texture for block \"{newBlock.Name}\"");
                    newBlock.BlockTexture = GetTexture("Default");
                }
            }
            
            _blockRegistry[type.Name] = newBlock;
        }
    }

    public static BlockType GetBlockTypeByName(string name)
    {
        if (DoesBlockTypeExistByName(name))
            return _blockRegistry[name];
        else
            throw new KeyNotFoundException($"Block type {name} not found");
    }

    public static bool DoesTextureExist(string textureName)
    {
        return _textureRegistry.ContainsKey(textureName);
    }
    
    public static BaseMaterial3D GetTexture(string textureName)
    {
        if (_textureRegistry.TryGetValue(textureName, out var texture))
            return texture;
        else throw new KeyNotFoundException($"Attempted to get texture that does not exist with name {textureName}");
    }
    
    private static void LoadTextures(string path)
    {
        if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
        
        using var dir = DirAccess.Open(path);

        if (dir == null) throw new FileNotFoundException($"Path is null: \"{path}\"");

        dir.ListDirBegin();
        string fileName = dir.GetNext();
        while (fileName != "")
        {
            string texturePath = Path.Join(dir.GetCurrentDir(), fileName);

            if (!texturePath.EndsWith(".bmp"))
            {
                fileName = dir.GetNext();
                continue;
            }

            GD.Print($"Loading texture: \"{texturePath}\"");
            var texture = GD.Load<Texture2D>(texturePath);
            if (texture == null) throw new FileNotFoundException($"Texture is null: \"{texturePath}\"");
            
            // Create a new material with the new texture (could be faster if we copied it from memory instead of disk every time
            var newMaterial = _standardBlockMaterial.Duplicate(true) as StandardMaterial3D;
            newMaterial.AlbedoTexture = texture;
            
            // finally add it to the registry
            _textureRegistry[Path.GetFileNameWithoutExtension(texturePath)] = newMaterial;
            
            fileName = dir.GetNext();
        }
    }
}