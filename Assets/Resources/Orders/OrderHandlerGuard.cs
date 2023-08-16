public class OrderHandlerGuard : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.IsTargetGameObject == false || (order.IsTargetGameObject == true && order.TargetGameObject != null);
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

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
