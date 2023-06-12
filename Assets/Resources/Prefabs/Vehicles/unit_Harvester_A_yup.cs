using UnityEngine;

public class unit_Harvester_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Load);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);
        Orders.AllowOrder(OrderType.Unload);
        Orders.AllowOrder(OrderType.Transport);

        Resources.Add("Coal", 0, 10);
        Resources.Add("Crystal", 0, 10);
        Resources.Add("Metal", 0, 10);
        Resources.Add("Metal Ore", 0, 10);
        Resources.Add("Wood", 0, 10);

        Speed = 4;
    }

    protected override void OnOrderIdle()
    {
        // TODO: Optimize.
        var game = GameObject.Find("Game").GetComponent<Game>();
        var order = game.CreateTransportOrder();

        if (order != null)
        {
            Orders.Add(order);
        }
    }
}
