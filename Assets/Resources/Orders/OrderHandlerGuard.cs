public class OrderHandlerGuard : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

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
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.IsTargetGameObject == false || (order.IsTargetGameObject == true && order.TargetGameObject != null && order.TargetGameObject != myGameObject);
    }
}
