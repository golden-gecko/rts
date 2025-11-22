using System.IO;

public class Harvester : Vehicle
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Construct);
        Orders.AllowOrder(OrderType.Gather);
        Orders.AllowOrder(OrderType.Load);
        Orders.AllowOrder(OrderType.Unload);
        Orders.AllowOrder(OrderType.Transport);

        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Barracks"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Factory_Heavy"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Factory_Light"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Radar_Outpost"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Refinery"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Research_Lab"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Spaceport"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Turret_Gun"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Turret_Missile"));
        Orders.AllowPrefab(Path.Combine(Config.DirectoryStructures, "Wall"));

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();

        GetComponent<Storage>().Resources.Add("Coal", 0, 10);
        GetComponent<Storage>().Resources.Add("Crystal", 0, 10);
        GetComponent<Storage>().Resources.Add("Iron", 0, 10);
        GetComponent<Storage>().Resources.Add("Iron Ore", 0, 10);
        GetComponent<Storage>().Resources.Add("Wood", 0, 10);

        Skills["Repair"] = new Repair("Repair", 3.0f, 4.0f, 20.0f);
    }
}
