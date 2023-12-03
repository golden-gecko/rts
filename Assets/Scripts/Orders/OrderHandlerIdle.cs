public class OrderHandlerIdle : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order;
        
        order = TryAttackerBehaviour(myGameObject); // TODO: Add priorities.

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = TryConstructorBehaviour(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = TryProducerBehaviour(myGameObject);

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

        order = TryGathererBehaviour(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = TryMinerBehaviour(myGameObject);

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

    private Order TryConstructorBehaviour(MyGameObject myGameObject)
    {
        Constructor constructor = myGameObject.GetComponentInChildren<Constructor>();

        if (constructor == null)
        {
            return null;
        }

        Order order = myGameObject.Player.GetJob(myGameObject, OrderType.Construct);

        if (order != null)
        {
            return order;
        }

        return null;
    }

    private Order TryGathererBehaviour(MyGameObject myGameObject)
    {
        Engine engine = myGameObject.GetComponentInChildren<Engine>();
        Storage storage = myGameObject.GetComponentInChildren<Storage>();

        if (engine == null || storage == null)
        {
            return null;
        }

        Order order = myGameObject.Player.GetJob(myGameObject, OrderType.GatherObject);

        if (order != null)
        {
            return order;
        }

        return null;
    }

    private Order TryMinerBehaviour(MyGameObject myGameObject)
    {
        Miner miner = myGameObject.GetComponentInChildren<Miner>();

        if (miner == null)
        {
            return null;
        }

        Order order = myGameObject.Player.GetJob(myGameObject, OrderType.MineObject);

        if (order != null)
        {
            return order;
        }

        return null;
    }

    private Order TryProducerBehaviour(MyGameObject myGameObject)
    {
        Producer producer = myGameObject.GetComponentInChildren<Producer>();

        if (producer == null)
        {
            return null;
        }

        Order order = Order.Produce();

        if (order != null)
        {
            return order;
        }

        return null;
    }

    private Order TryWorkerBehaviour(MyGameObject myGameObject)
    {
        Engine engine = myGameObject.GetComponentInChildren<Engine>();
        Storage storage = myGameObject.GetComponentInChildren<Storage>();

        if (engine == null || storage == null)
        {
            return null;
        }

        Order order;
        
        order = myGameObject.Player.GetJob(myGameObject, OrderType.Unload);

        if (order != null)
        {
            return order;
        }

        order = myGameObject.Player.GetJob(myGameObject, OrderType.Transport);

        if (order != null)
        {
            return order;
        }

        return null;
    }
}
