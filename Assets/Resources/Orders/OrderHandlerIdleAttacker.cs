public class OrderHandlerIdleAttacker : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order;

        if (myGameObject.GetComponent<Gun>() == null)
        {
            return;
        }

        order = Game.Instance.CreataAttackJob(myGameObject);

        if (order == null)
        {
            return;
        }

        myGameObject.Orders.Add(order);
    }
}
