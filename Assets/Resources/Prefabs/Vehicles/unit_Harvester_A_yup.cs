using UnityEngine;

public class unit_Harvester_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Construct);
        Orders.AllowOrder(OrderType.Load);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);
        Orders.AllowOrder(OrderType.Unload);
        Orders.AllowOrder(OrderType.Transport);

        Orders.AllowPrefab("Prefabs/Buildings/struct_Barracks_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Factory_Heavy_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Factory_Light_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Misc_Building_B_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Radar_Outpost_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Refinery_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Research_Lab_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Spaceport_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Turret_Gun_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Turret_Missile_A_yup");
        Orders.AllowPrefab("Prefabs/Buildings/struct_Wall_A_yup");

        Resources.Add("Coal", 0, 10);
        Resources.Add("Crystal", 0, 10);
        Resources.Add("Metal", 0, 10);
        Resources.Add("Metal Ore", 0, 10);
        Resources.Add("Wood", 0, 10);

        Speed = 4;
    }

    protected override void OnOrderConstruct()
    {
        var order = Orders.First();

        if (order.TargetGameObject.IsConstructed())
        {
            order.Timer.Update(Time.deltaTime);

            if (order.Timer.Finished)
            {
                order.TargetGameObject.State = MyGameObjectState.Operational;
                order.Timer.Reset();

                Orders.Pop();

                Stats.Add(Stats.OrdersExecuted, 1);
                Stats.Add(Stats.TimeConstructing, order.Timer.Max);
            }
        }
        else if (IsCloseTo(order.TargetGameObject.Entrance) == false)
        {
            Move(order.TargetGameObject, 0);
        }
        else
        {
            // TODO: Wait for working to bring resources.
            Wait(0);

            /*
            TODO: Fix this logic by keeping the priority when decomposing Transport order.

            var game = GameObject.Find("Game").GetComponent<Game>();
            var newOrder = game.CreateTransport(null, order.TargetGameObject);

            if (order != null)
            {
                Orders.Insert(0, newOrder);
            }
            else
            {
                Wait(0);
            }
            */
        }
    }

    protected override void OnOrderIdle()
    {
        var game = GameObject.Find("Game").GetComponent<Game>();

        Order order;

        order = game.CreateUnload();

        if (order != null)
        {
            Orders.Add(order);

            return;
        }

        order = game.CreateTransport();

        if (order != null)
        {
            Orders.Add(order);

            return;
        }

        order = game.CreateConstruction();

        if (order != null)
        {
            Orders.Add(order);

            return;
        }
    }
}
