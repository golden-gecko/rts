using System.IO;

public class Factory_Light : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Grav_Light"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Harvester"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Quad"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Trike"));
    }
}
