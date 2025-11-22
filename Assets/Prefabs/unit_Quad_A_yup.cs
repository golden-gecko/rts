using UnityEngine;

public class unit_Quad_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Move);

    }

    protected override void Update()
    {
        base.Update();
    }
}
