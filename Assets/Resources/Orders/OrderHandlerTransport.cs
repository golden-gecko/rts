public class OrderHandlerTransport : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.SourceGameObject != null && order.TargetGameObject != null;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        myGameObject.Orders.Pop(); // TODO: Order is reversed because of gather order.

        myGameObject.Unload(order.TargetGameObject, order.Resources, 0);
        myGameObject.Move(order.TargetGameObject.Entrance, 0);
        myGameObject.Load(order.SourceGameObject, order.Resources, 0);
        myGameObject.Move(order.SourceGameObject.Entrance, 0);
    }
}
