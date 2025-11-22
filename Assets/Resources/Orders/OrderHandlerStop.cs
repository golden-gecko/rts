public class OrderHandlerStop : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Stats.Add(Stats.OrdersCancelled, myGameObject.Orders.Count);
        myGameObject.Orders.Clear();
    }
}
