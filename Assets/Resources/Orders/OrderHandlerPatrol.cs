public class OrderHandlerPatrol : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.TargetPosition);
        myGameObject.Move(myGameObject.Position);

        myGameObject.Orders.MoveToEnd();
    }
}
