public class OrderHandlerGather : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (order.IsTargetGameObject)
        {
            ProcessTargetObject(myGameObject, order);
        }
        else if (order.Resource != null && order.Resource.Length > 0)
        {
            ProcessResource(myGameObject, order);
        }
        else
        {
            Process(myGameObject);
        }
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return (order.IsTargetGameObject == true && order.TargetGameObject != null && order.TargetGameObject != myGameObject) || (order.IsTargetGameObject == false);
    }

    private void ProcessTargetObject(MyGameObject myGameObject, Order order)
    {
        if (order.TargetGameObject == null)
        {
            Fail(myGameObject);

            return;
        }

        MyResource myResource = order.TargetGameObject.GetComponent<MyResource>();

        if (myResource == null)
        {
            Fail(myGameObject);

            return;
        }

        MyGameObject storage = null;
        Resource resource = null;

        foreach (Resource i in myResource.GetComponent<Storage>().Resources.Items)
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

    private void ProcessResource(MyGameObject myGameObject, Order order)
    {
        MyResource myResource = myGameObject.Player.GetResource(myGameObject, order.Resource);

        if (myResource == null)
        {
            Fail(myGameObject);

            return;
        }

        int current = myResource.GetComponent<Storage>().Resources.Current(order.Resource);

        if (current <= 0)
        {
            Fail(myGameObject);

            return;
        }

        MyGameObject storage = myGameObject.Player.GetStorage(myGameObject, order.Resource, current);

        if (storage != null)
        {
            Fail(myGameObject);

            return;
        }

        GatherFromObject(myGameObject, myResource, storage, order.Resource, current);
    }

    private void Process(MyGameObject myGameObject)
    {
        MyResource myResource = myGameObject.Player.GetResource(myGameObject);

        if (myResource == null)
        {
            Fail(myGameObject);

            return;
        }

        MyGameObject storage = null;
        Resource resource = null;

        foreach (Resource i in myResource.GetComponent<Storage>().Resources.Items)
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

        GatherFromObject(myGameObject, myResource, storage, resource.Name, resource.Current);
    }

    private void GatherFromObject(MyGameObject myGameObject, MyGameObject myResource, MyGameObject storage, string resource, int value)
    {
        if (myResource == null || storage == null)
        {
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();

            return;
        }

        myGameObject.Orders.Pop();
        myGameObject.Transport(myResource, storage, resource, value);
    }
}
