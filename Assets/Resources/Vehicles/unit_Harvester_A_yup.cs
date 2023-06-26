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

        Resources.Add("Coal", 0, 10);
        Resources.Add("Crystal", 0, 10);
        Resources.Add("Metal", 0, 10);
        Resources.Add("Metal Ore", 0, 10);
        Resources.Add("Wood", 0, 10);

        Speed = 4.0f;
        Health = 50.0f;
        MaxHealth = 50.0f;
    }

    protected override void OnOrderIdle()
    {
        Game game = GameObject.Find("Game").GetComponent<Game>();

        Order order;

        order = game.CreateOrderUnload();

        if (order != null)
        {
            Orders.Add(order);

            return;
        }

        order = game.CreateOrderTransport();

        if (order != null)
        {
            Orders.Add(order);

            return;
        }

        order = game.CreateOrderConstruction();

        if (order != null)
        {
            Orders.Add(order);

            return;
        }
    }
}
