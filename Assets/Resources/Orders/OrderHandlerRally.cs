public class OrderHandlerRally : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.GetComponent<Constructor>().RallyPoint = order.TargetPosition;
        myGameObject.Orders.Pop();
    }
}
