public class OrderHandlerRally : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.RallyPoint = order.TargetPosition;

        myGameObject.Orders.Pop();
    }
}
