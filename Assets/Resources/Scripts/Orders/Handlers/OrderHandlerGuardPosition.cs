public class OrderHandlerGuardPosition : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.TargetPosition, 0);
    }
}
