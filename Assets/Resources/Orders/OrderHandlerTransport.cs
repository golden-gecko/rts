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

        myGameObject.Orders.Pop();

        myGameObject.Move(order.SourceGameObject.Entrance);
        myGameObject.Load(order.SourceGameObject, order.Resource, order.Value);
        myGameObject.Move(order.TargetGameObject.Entrance);
        myGameObject.Unload(order.TargetGameObject, order.Resource, order.Value);
    }

    protected override bool IsValid(Order order)
    {
        return order.SourceGameObject != null && order.TargetGameObject != null;
    }
}
