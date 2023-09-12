public class OrderHandlerStop : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Orders.Pop();
        myGameObject.Stats.Add(Stats.OrdersCancelled, myGameObject.Orders.Count);
        myGameObject.Orders.Clear();
    }
}
