using UnityEngine;

public class unit_Quad_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Move);
        Orders.Allow(OrderType.Patrol);
    }
}
