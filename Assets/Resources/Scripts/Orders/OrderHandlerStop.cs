public class OrderHandlerStop : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Orders.Pop();
        myGameObject.ClearOrders();
    }
}
