using System.IO;

public class Harvester : Vehicle
{
    protected override void Awake()
    {
        base.Awake();

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

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker(); // TODO: Move to component.

        Skills["Repair"] = new Repair("Repair", 3.0f, 4.0f, 20.0f);
    }
}
