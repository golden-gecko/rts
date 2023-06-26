public class OrderHandlerFollow : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.TargetGameObject);

        myGameObject.Orders.MoveToEnd();
    }
}
