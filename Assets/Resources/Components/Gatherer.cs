using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Gatherer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.Gather);

        Parent.OrderHandlers[OrderType.Gather] = new OrderHandlerGather();
    }

    public override string GetInfo()
    {
        return string.Format("Gatherer");
    }
}
