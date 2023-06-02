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

        // TODO: Move to ResourceContainer.
        Resources.Add("Coal", new Resource("Coal", 0, 20));
        Resources.Add("Metal", new Resource("Coal", 0, 20));
        Resources.Add("Metal Ore", new Resource("Coal", 0, 20));
        Resources.Add("Wood", new Resource("Coal", 0, 20));
    }

    #region Handlers
    protected override void OnOrderIdle()
    {
        /*
        var objects = GameObject.FindGameObjectsWithTag("Resource");

        foreach (var item in objects)
        {
            Orders.Add(new Order(OrderType.Move, item.transform.position));

            break;
        }
        */

        Orders.Pop();
    }
    #endregion
}
