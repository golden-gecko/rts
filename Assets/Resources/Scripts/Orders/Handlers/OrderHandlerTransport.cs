public class OrderHandlerTransport : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        myGameObject.Orders.Pop();

        myGameObject.Load(order.SourceGameObject, order.Resource, order.Value, 0);
        myGameObject.Unload(order.TargetGameObject, order.Resource, order.Value, 1);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.SourceGameObject != null && order.SourceGameObject != myGameObject && order.TargetGameObject != null && order.TargetGameObject != myGameObject && order.Resource != "" && order.Value > 0;
    }
}
