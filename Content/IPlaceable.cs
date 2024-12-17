namespace Krystal.World;

public interface IPlaceable
{
    void OnUpdate();
    
    void OnPlace();
    void OnDestroy();
}