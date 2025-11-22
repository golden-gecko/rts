public class OrderHandlerMineObject : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (order.TargetGameObject == null)
        {
            Success(myGameObject);

            return;
        }

        ProcessTargetObject(myGameObject, order);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.TargetGameObject != null && order.TargetGameObject != myGameObject;
    }

    private void ProcessTargetObject(MyGameObject myGameObject, Order order)
    {
        if (order.TargetGameObject == null)
        {
            Fail(myGameObject);

            return;
        }

        MyGameObject storage = null;
        Resource resource = null;

        Storage targetGameObjectStorage = order.TargetGameObject.GetComponentInChildren<Storage>();

        if (targetGameObjectStorage == false)
        {
            Fail(myGameObject);

            return;
        }

        foreach (Resource i in targetGameObjectStorage.Resources.Items)
        {
            storage = myGameObject.Player.GetStorage(myGameObject, i.Name, i.Current);
            resource = i;

            if (storage != null)
            {
                break;
            }
        }

        if (storage == null)
        {
            Fail(myGameObject);

            return;
        }

        GatherFromObject(myGameObject, order.TargetGameObject, storage, resource.Name, resource.Current);
    }

    private void GatherFromObject(MyGameObject myGameObject, MyGameObject myResource, MyGameObject storage, string resource, int value)
    {
        if (myResource == null || storage == null)
        {
            Fail(myGameObject);

            return;
        }

        Miner miner = myGameObject.GetComponentInChildren<Miner>();

        if (miner == null)
        {
            Fail(myGameObject);

            return;
        }

        myGameObject.Orders.Pop();
        myGameObject.Load(myResource, miner.Range, resource, value);
    }
}
