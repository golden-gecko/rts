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
        Gatherer gatherer = myGameObject.GetComponentInChildren<Gatherer>();
        Storage storage = myGameObject.GetComponentInChildren<Storage>();

        if (drive != null && gatherer != null && storage != null)
        {
            Order order = myGameObject.Player.GetJob(myGameObject, OrderType.GatherObject);

            if (order != null)
            {
                return order;
            }
        }
        else if (drive != null && storage != null)
        {
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
        }
        else if (gatherer != null && storage != null)
        {
            MyGameObject resourceInRange = myGameObject.Player.GetResourceToGatherInRange(myGameObject, myGameObject.GetComponentInChildren<Gatherer>().Range);
            Order order = Order.GatherObject(resourceInRange);

            if (order != null)
            {
                return order;
            }
        }

        return null;
    }
}
