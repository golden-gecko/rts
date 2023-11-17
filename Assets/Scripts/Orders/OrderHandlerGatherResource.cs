public class OrderHandlerGatherResource : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        ProcessResource(myGameObject, order);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.Resource != null && order.Resource.Length > 0;
    }

    private void ProcessResource(MyGameObject myGameObject, Order order)
    {
        MyGameObject myResource = myGameObject.Player.GetResource(myGameObject, order.Resource);

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
