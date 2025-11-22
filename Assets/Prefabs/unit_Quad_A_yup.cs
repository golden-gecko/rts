using UnityEngine;

public class unit_Quad_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Move);
        Orders.Allow(OrderType.Patrol);
    }

    protected override void OnIdleOrder()
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
}
