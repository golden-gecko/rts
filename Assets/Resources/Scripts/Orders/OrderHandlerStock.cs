public class OrderHandlerStock : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        MyGameObject producer = myGameObject.Player.GetProducer(myGameObject, order.Resource, order.Value);

        if (producer == null)
        {
            Fail(myGameObject);

            return;
        }

        myGameObject.Orders.Pop();
        myGameObject.Transport(producer, order.TargetGameObject, order.Resource, order.Value, 0);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.TargetGameObject != null && order.TargetGameObject != myGameObject && order.Resource != "" && order.Value > 0;
    }
}
