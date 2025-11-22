public class OrderHandlerDestroy : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.OnDestroy_();
        myGameObject.Orders.Pop();
    }
}
