using System.IO;

public class Barracks : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Infantry_Light"));
    }
}
