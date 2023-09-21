public class OrderHandlerPatrol : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.TargetPosition, 0);
        myGameObject.Move(myGameObject.Position, 1);
    }
}
