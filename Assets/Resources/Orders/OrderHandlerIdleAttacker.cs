public class OrderHandlerIdleAttacker : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order;

        if (myGameObject.GetComponent<Gun>() == null)
        {
            return;
        }

        order = Game.Instance.CreateAttackJob(myGameObject);

        if (order == null)
        {
            return;
        }

        myGameObject.Orders.Add(order);
    }
}
