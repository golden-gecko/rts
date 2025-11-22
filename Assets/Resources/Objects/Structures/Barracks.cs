using System.IO;

public class Barracks : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Infantry_Light"));

        GetComponent<Storage>().Resources.Add("Iron", 0, 40, ResourceDirection.In);
    }
}
