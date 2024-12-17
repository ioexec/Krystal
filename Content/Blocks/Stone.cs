namespace Krystal.World.Blocks;

public class Stone : BlockType
{
    public override void SetDefaults()
    {
        Name = "Stone";
        BlockTexture = ContentManager.GetTexture("Stone");
    }
}