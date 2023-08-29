public class OrderHandlerIdleWorker : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order;
        
        // TODO: Find unload job.

        order = myGameObject.Player.CreateOrderTransport(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = myGameObject.Player.CreateOrderGather(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = myGameObject.Player.CreateOrderConstruction(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        myGameObject.Orders.Add(Order.Wait());
    }
}
