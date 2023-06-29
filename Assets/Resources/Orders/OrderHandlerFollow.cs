public class OrderHandlerFollow : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (myGameObject.IsCloseTo(order.TargetGameObject.Position) == false)
        {
            myGameObject.Move(order.TargetGameObject.Position);
        }

        myGameObject.Orders.MoveToEnd();
    }
}
