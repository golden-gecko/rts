public class OrderHandlerGuard : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.IsTargetGameObject)
        {
            myGameObject.Follow(order.TargetGameObject);
        }
        else
        {
            myGameObject.Move(order.TargetPosition);
        }

        myGameObject.Orders.MoveToEnd();
    }
}
