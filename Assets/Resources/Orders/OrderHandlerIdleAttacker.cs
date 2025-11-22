public class OrderHandlerIdleAttacker : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Player.GetJob(myGameObject, OrderType.Attack);

        if (order == null)
        {
            return;
        }

        myGameObject.Orders.Add(order);
    }
}
