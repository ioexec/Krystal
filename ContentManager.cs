using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using System.Reflection;

using Krystal.World;
using EditorFileSystemImportFormatSupportQuery = Godot.EditorFileSystemImportFormatSupportQuery;

namespace Krystal;

/// <summary>
/// Serves as Krystal's main management class for different content in the game. This includes textures, blocks, items, mobs.
/// </summary>
public static class ContentManager
{
    private static Dictionary<string, StandardMaterial3D> _textureRegistry;
    private static Dictionary<string, BlockType> _blockRegistry;
    private static StandardMaterial3D _standardBlockMaterial;
    
    
    static ContentManager()
    {
        _textureRegistry = new Dictionary<string, StandardMaterial3D>();
        _standardBlockMaterial = GD.Load<StandardMaterial3D>("res://Resources/Materials/StandardBlockMaterial.tres");
        LoadTextures("Resources/Textures");

        _blockRegistry = new Dictionary<string, BlockType>();
        LoadBlocks();
        return;
    }
    
    /// <summary>
    /// Returns true if a block with this name exists in the block registry.
    /// </summary>
    /// <param name="name">The internal name of the block. i.e "Grass"</param>
    /// <returns></returns>
    public static bool DoesBlockTypeExistByName(string name)
    {
        return _blockRegistry.ContainsKey(name);
    }
    
    /// <summary>
    /// Load all blocks defined in Krystal.World.Blocks as instances in the block registry
    /// </summary>
    /// <exception cref="Exception">a block failed to instantiate</exception>
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
            // Create an instance of the BlockType
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
                    // Resort to default texture if one could not be found
                    GD.Print($"Could not find texture for block \"{newBlock.Name}\"");
                    newBlock.BlockTexture = GetTexture("Default");
                }
            }
            
            _blockRegistry[type.Name] = newBlock;
        }
    }

    /// <summary>
    /// Returns a  <c>BlockType</c> registered under the provided internal name.
    /// </summary>
    /// <param name="name">The internal name of the block. i.e: "Grass"</param>
    /// <returns><c>BlockType</c> loaded under the given name</returns>
    /// <exception cref="KeyNotFoundException">A <c>BlockType</c> registered under the given name was not found</exception>
    public static BlockType GetBlockTypeByName(string name)
    {
        if (DoesBlockTypeExistByName(name))
            return _blockRegistry[name];
        else
            throw new KeyNotFoundException($"Block type {name} not found");
    }

    /// <summary>
    /// Checks if a texture has been registered under the given internal name
    /// </summary>
    /// <param name="textureName">the file name of the texture without its extension. i.e "me.bmp" -> "me"</param>
    /// <returns></returns>
    public static bool DoesTextureExist(string textureName)
    {
        return _textureRegistry.ContainsKey(textureName);
    }
    
    /// <summary>
    /// Obtain texture object registered under internal name
    /// </summary>
    /// <param name="textureName">the file name of the texture without its extension. i.e "me.bmp" -> "me"</param>
    /// <returns>A <c>BaseMaterial3D</c> reference that represents the texture</returns>
    public static BaseMaterial3D GetTexture(string textureName)
    {
        if (_textureRegistry.TryGetValue(textureName, out var texture))
            return texture;
        else throw new KeyNotFoundException($"Attempted to get texture that does not exist with name {textureName}");
    }
    
    /// <summary>
    /// Loads all textures with the correct extensions into the texture registry and assigns their names
    /// as their filename without the extension.
    /// </summary>
    /// <param name="path">The directory to search for textures</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
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