public class OrderHandlerPatrol : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.TargetPosition);
        myGameObject.Move(myGameObject.Position);

        myGameObject.Orders.MoveToEnd();
    }
}
