using UnityEngine;

[RequireComponent(typeof(Storage))]
public class Gatherer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        Parent.Orders.AllowOrder(OrderType.GatherObject);
        Parent.Orders.AllowOrder(OrderType.GatherResource);

        Parent.OrderHandlers[OrderType.GatherObject] = new OrderHandlerGatherObject();
        Parent.OrderHandlers[OrderType.GatherResource] = new OrderHandlerGatherResource();
    }

    public override string GetInfo()
    {
        return string.Format("Gatherer");
    }
}
