using UnityEngine;

public class unit_Harvester_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Construct);
        Orders.AllowOrder(OrderType.Load);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);
        Orders.AllowOrder(OrderType.Unload);
        Orders.AllowOrder(OrderType.Transport);

        Orders.AllowPrefab("Buildings/struct_Barracks_A_yup");
        Orders.AllowPrefab("Buildings/struct_Factory_Heavy_A_yup");
        Orders.AllowPrefab("Buildings/struct_Factory_Light_A_yup");
        Orders.AllowPrefab("Buildings/struct_Misc_Building_B_yup");
        Orders.AllowPrefab("Buildings/struct_Radar_Outpost_A_yup");
        Orders.AllowPrefab("Buildings/struct_Refinery_A_yup");
        Orders.AllowPrefab("Buildings/struct_Research_Lab_A_yup");
        Orders.AllowPrefab("Buildings/struct_Spaceport_A_yup");
        Orders.AllowPrefab("Buildings/struct_Turret_Gun_A_yup");
        Orders.AllowPrefab("Buildings/struct_Turret_Missile_A_yup");
        Orders.AllowPrefab("Buildings/struct_Wall_A_yup");

        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleWorker();

        Resources.Add("Coal", 0, 10);
        Resources.Add("Crystal", 0, 10);
        Resources.Add("Metal", 0, 10);
        Resources.Add("Metal Ore", 0, 10);
        Resources.Add("Wood", 0, 10);

        Health = 50.0f;
        MaxHealth = 50.0f;
        LoadTime = 2.0f;
        Speed = 4.0f;
        UnloadTime = 2.0f;
        WaitTime = 2.0f;
    }
}
