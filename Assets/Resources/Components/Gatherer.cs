using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Gatherer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        parent.Orders.AllowOrder(OrderType.Gather);

        parent.OrderHandlers[OrderType.Gather] = new OrderHandlerGather();
    }

    public override string GetInfo()
    {
        return string.Format("Gatherer");
    }
}
