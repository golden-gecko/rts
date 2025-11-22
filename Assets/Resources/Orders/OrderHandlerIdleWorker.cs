public class OrderHandlerIdleWorker : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Orders.Add(Order.Wait());

        Order order = myGameObject.Player.GetJob(myGameObject, OrderType.Unload);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = myGameObject.Player.GetJob(myGameObject, OrderType.Transport);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = myGameObject.Player.GetJob(myGameObject, OrderType.Gather);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = myGameObject.Player.GetJob(myGameObject, OrderType.Construct);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }
    }
}
