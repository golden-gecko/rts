public class OrderHandlerFollow : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (myGameObject.IsCloseTo(order.TargetGameObject.Entrance) == false)
        {
            myGameObject.Move(order.TargetGameObject.Entrance);
        }

        myGameObject.Orders.MoveToEnd();
    }

    protected override bool IsValid(Order order)
    {
        return order.TargetGameObject != null;
    }
}
