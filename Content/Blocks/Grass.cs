namespace Krystal.Content.Blocks;

public class Grass : BlockType
{
    public override void SetDefaults()
    {
        base.SetDefaults();
        this.BlockTexture = ContentManager.GetTexture("Grass");
    }
}