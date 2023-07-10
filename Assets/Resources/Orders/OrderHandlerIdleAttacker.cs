public class OrderHandlerIdleAttacker : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order;

        order = myGameObject.Player.CreataAttackJob(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }
    }
}
