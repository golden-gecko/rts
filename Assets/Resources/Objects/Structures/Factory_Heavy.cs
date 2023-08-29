using System.IO;

public class Factory_Heavy : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Assemble); // TODO: Move to component.
        Orders.AllowOrder(OrderType.Rally);

        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Tank_Combat"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryUnits, "Tank_Missile"));

        GetComponent<Storage>().Resources.Add("Iron", 0, 40, ResourceDirection.In);
    }
}
