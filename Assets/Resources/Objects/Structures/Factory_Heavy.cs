using System.IO;

public class Factory_Heavy : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Tank_Combat"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Tank_Missile"));
    }
}
