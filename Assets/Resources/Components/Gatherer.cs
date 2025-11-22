using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Gatherer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Gather);

        parent.OrderHandlers[OrderType.Gather] = new OrderHandlerGather();
    }
}
