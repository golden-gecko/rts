public class Harvester : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Construct);
        Orders.AllowOrder(OrderType.Gather);
        Orders.AllowOrder(OrderType.Load);
        Orders.AllowOrder(OrderType.Unload);
        Orders.AllowOrder(OrderType.Transport);

        Orders.AllowPrefab("Objects/Structures/Barracks");
        Orders.AllowPrefab("Objects/Structures/Factory_Heavy");
        Orders.AllowPrefab("Objects/Structures/Factory_Light");
        Orders.AllowPrefab("Objects/Structures/Radar_Outpost");
        Orders.AllowPrefab("Objects/Structures/Refinery");
        Orders.AllowPrefab("Objects/Structures/Research_Lab");
        Orders.AllowPrefab("Objects/Structures/Spaceport");
        Orders.AllowPrefab("Objects/Structures/Turret_Gun");
        Orders.AllowPrefab("Objects/Structures/Turret_Missile");
        Orders.AllowPrefab("Objects/Structures/Wall");

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();

        Resources.Add("Coal", 0, 10);
        Resources.Add("Crystal", 0, 10);
        Resources.Add("Metal", 0, 10);
        Resources.Add("Metal Ore", 0, 10);
        Resources.Add("Wood", 0, 10);

        Skills["Repair"] = new Repair("Repair", 3.0f, 4.0f, 20.0f);
    }
}
