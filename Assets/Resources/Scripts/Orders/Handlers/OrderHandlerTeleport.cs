public class OrderHandlerTeleport : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.SourceGameObject != null && Utils.IsCloseTo(myGameObject.Position, order.SourceGameObject.Entrance) == false)
        {
            myGameObject.Move(order.SourceGameObject.Entrance, 0);

            order.SourceGameObject = null;

            return;
        }

        myGameObject.Position = order.TargetGameObject.Exit;

        if (order.TargetGameObject != null && Utils.IsCloseTo(myGameObject.Position, order.TargetGameObject.Exit) == false)
        {
            myGameObject.Move(order.TargetGameObject.Exit, 0);

            order.TargetGameObject = null;

            return;
        }

        Success(myGameObject);
    }
}
