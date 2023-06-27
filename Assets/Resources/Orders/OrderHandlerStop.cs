public class OrderHandlerStop : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Orders.Clear();
    }
}
