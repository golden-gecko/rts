public class Gatherer : MyComponent
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().Orders.AllowOrder(OrderType.Gather);

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Gather] = new OrderHandlerGather();
    }
}
