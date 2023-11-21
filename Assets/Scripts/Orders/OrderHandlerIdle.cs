public class OrderHandlerIdle : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = TryAttackerBehaviour(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = TryWorkerBehaviour(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }
    }

    private Order TryAttackerBehaviour(MyGameObject myGameObject)
    {
        Gun gun = myGameObject.GetComponentInChildren<Gun>();

        if (gun == null)
        {
            return null;
        }

        Order order = myGameObject.Player.GetJob(myGameObject, OrderType.AttackObject);

        if (order != null)
        {
            return order;
        }

        return null;
    }

    private Order TryWorkerBehaviour(MyGameObject myGameObject)
    {
        Drive drive = myGameObject.GetComponentInChildren<Drive>();
        Storage storage = myGameObject.GetComponentInChildren<Storage>();

        if (drive == null || storage == null)
        {
            return null;
        }

        Order order = myGameObject.Player.GetJob(myGameObject, OrderType.Unload);

        if (order != null)
        {
            return order;
        }

        order = myGameObject.Player.GetJob(myGameObject, OrderType.Construct);

        if (order != null)
        {
            return order;
        }

        order = myGameObject.Player.GetJob(myGameObject, OrderType.Transport);

        if (order != null)
        {
            return order;
        }

        order = myGameObject.Player.GetJob(myGameObject, OrderType.GatherObject);

        if (order != null)
        {
            return order;
        }

        return null;
    }
}
