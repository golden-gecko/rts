public class OrderHandlerTransport : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.SourceGameObject);
        myGameObject.Load(order.SourceGameObject, order.Resources);
        myGameObject.Move(order.TargetGameObject);
        myGameObject.Unload(order.TargetGameObject, order.Resources);

        myGameObject.Orders.Pop();
    }
}
