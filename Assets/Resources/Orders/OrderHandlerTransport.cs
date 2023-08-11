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
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();

            return;
        }

        myGameObject.Move(order.SourceGameObject.Entrance);
        myGameObject.Load(order.SourceGameObject, order.Resources);
        myGameObject.Move(order.TargetGameObject.Entrance);
        myGameObject.Unload(order.TargetGameObject, order.Resources);

        myGameObject.Orders.Pop();
    }
}
