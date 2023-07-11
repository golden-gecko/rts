public class OrderHandlerIdleAttacker : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order;

        order = Game.Instance.CreataAttackJob(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }
    }
}
