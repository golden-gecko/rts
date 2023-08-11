public class OrderHandlerFollow : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.TargetGameObject != null;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();

            return;
        }

        if (myGameObject.IsCloseTo(order.TargetGameObject.Position) == false)
        {
            myGameObject.Move(order.TargetGameObject.Position);
        }

        myGameObject.Orders.MoveToEnd();
    }
}
