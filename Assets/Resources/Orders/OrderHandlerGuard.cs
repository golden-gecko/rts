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
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
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
