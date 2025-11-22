using System.Collections.Generic;
using UnityEngine;

public class unit_Harvester_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Load);
        Orders.Allow(OrderType.Move);
        Orders.Allow(OrderType.Patrol);
        Orders.Allow(OrderType.Unload);
        Orders.Allow(OrderType.Transport);

        Resources.Add("Coal", 0, 20);
        Resources.Add("Metal", 0, 20);
        Resources.Add("Metal Ore", 0, 20);
        Resources.Add("Wood", 0, 20);
    }
}
