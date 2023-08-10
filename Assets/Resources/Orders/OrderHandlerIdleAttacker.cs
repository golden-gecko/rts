public class OrderHandlerIdleAttacker : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order;

        if (myGameObject.Gun != null)
        {
            order = Game.Instance.CreataAttackJob(myGameObject);

            if (order != null)
            {
                myGameObject.Orders.Add(order);

                return;
            }
        }
    }
}
