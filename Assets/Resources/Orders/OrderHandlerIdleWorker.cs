public class OrderHandlerIdleWorker : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order;
        
        // TODO: Find unload job.

        order = Game.Instance.CreateOrderTransport(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = Game.Instance.CreateOrderConstruction(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        myGameObject.Orders.Add(Order.Wait(myGameObject.WaitTime));
    }
}
