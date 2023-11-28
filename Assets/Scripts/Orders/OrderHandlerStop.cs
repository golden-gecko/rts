public class OrderHandlerStop : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        myGameObject.Orders.Pop();
        myGameObject.ClearOrders();
    }
}
