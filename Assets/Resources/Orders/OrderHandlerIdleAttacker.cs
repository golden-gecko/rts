public class OrderHandlerIdleAttacker : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order;

        if (myGameObject.GetComponent<Gun>() != null)
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
