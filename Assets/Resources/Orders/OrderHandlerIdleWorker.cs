public class OrderHandlerIdleWorker : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Orders.Add(Order.Wait());

        Order order = myGameObject.Player.CreateOrderUnload(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

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
    }
}
