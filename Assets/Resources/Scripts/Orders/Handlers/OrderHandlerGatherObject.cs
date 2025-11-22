public class OrderHandlerGatherObject : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
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

    private void GatherFromObject(MyGameObject myGameObject, MyGameObject myResource, MyGameObject storage, string resource, int value)
    {
        if (myResource == null || storage == null)
        {
            Fail(myGameObject);

            return;
        }

        myGameObject.Orders.Pop();
        myGameObject.Transport(myResource, storage, resource, value);
    }
}
