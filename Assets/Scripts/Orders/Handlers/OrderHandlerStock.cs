public class OrderHandlerStock : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        MyGameObject source = myGameObject.Player.GetProducer(myGameObject, order.Resource, order.Value);

        if (source == null)
        {
            source = myGameObject.Player.GetResourceToGather(myGameObject, order.Resource, order.Value);

            if (source != null)
            {
                Fail(myGameObject);

                return;
            }
        }

        myGameObject.Orders.Pop();
        myGameObject.Transport(source, order.TargetGameObject, order.Resource, order.Value, 0);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.TargetGameObject != null && order.TargetGameObject != myGameObject && order.Resource != "" && order.Value > 0;
    }
}
