public class OrderHandlerStop : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Stats.Add(Stats.OrdersCancelled, myGameObject.Orders.Count);
        myGameObject.Orders.Clear();
    }
}
