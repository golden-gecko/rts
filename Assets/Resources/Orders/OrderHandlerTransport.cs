public class OrderHandlerTransport : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            Fail(myGameObject);

            return;
        }

        myGameObject.Orders.Pop(); // TODO: Order is reversed because of gather order.

        myGameObject.Unload(order.TargetGameObject, order.Resource, order.Value, 0);
        myGameObject.Move(order.TargetGameObject.Entrance, 0);
        myGameObject.Load(order.SourceGameObject, order.Resource, order.Value, 0);
        myGameObject.Move(order.SourceGameObject.Entrance, 0);
    }

    protected override bool IsValid(Order order)
    {
        return order.SourceGameObject != null && order.TargetGameObject != null;
    }
}
