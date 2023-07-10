public class unit_Harvester_A_yup : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Construct);
        Orders.AllowOrder(OrderType.Load);
        Orders.AllowOrder(OrderType.Unload);
        Orders.AllowOrder(OrderType.Transport);

        Orders.AllowPrefab("Objects/Structures/struct_Barracks_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Factory_Heavy_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Factory_Light_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Misc_Building_B_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Radar_Outpost_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Refinery_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Research_Lab_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Spaceport_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Turret_Gun_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Turret_Missile_A_yup");
        Orders.AllowPrefab("Objects/Structures/struct_Wall_A_yup");

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();

        Resources.Add("Coal", 0, 10);
        Resources.Add("Crystal", 0, 10);
        Resources.Add("Metal", 0, 10);
        Resources.Add("Metal Ore", 0, 10);
        Resources.Add("Wood", 0, 10);

        LoadTime = 2.0f;
        MissileRange = 0.0f;
        Speed = 4.0f;
    }
}
