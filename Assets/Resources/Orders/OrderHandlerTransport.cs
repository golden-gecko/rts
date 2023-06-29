public class OrderHandlerTransport : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.SourceGameObject.Entrance);
        myGameObject.Load(order.SourceGameObject, order.Resources);
        myGameObject.Move(order.TargetGameObject.Entrance);
        myGameObject.Unload(order.TargetGameObject, order.Resources);

        myGameObject.Orders.Pop();
    }
}
