public class OrderHandlerGuard : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            myGameObject.Move(order.TargetPosition);
        }
        else
        {
            myGameObject.Move(order.TargetGameObject);
        }

        myGameObject.Orders.MoveToEnd();
    }
}
