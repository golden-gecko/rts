using System.Linq;

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

            MyGameObject storage = myGameObject.Player.GetStorage(myGameObject, myResource);

            if (storage == null)
            {
                Fail(myGameObject);

                return;
            }

            GatherFromObject(myGameObject, order.TargetGameObject, storage);
        }
        else
        {
            MyResource myResource = myGameObject.Player.GetResource(myGameObject, order.Resource);

            if (myResource == null)
            {
                Fail(myGameObject);

                return;
            }

            MyGameObject storage = myGameObject.Player.GetStorage(myGameObject, myResource);

            if (storage == null)
            {
                Fail(myGameObject);

                return;
            }

            GatherFromObject(myGameObject, myResource, storage);
        }
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return (order.IsTargetGameObject == true && order.TargetGameObject != myGameObject) || order.Resource.Length > 0;
    }

    private void GatherFromObject(MyGameObject myGameObject, MyGameObject myResource, MyGameObject storage)
    {
        if (myResource == null || storage == null)
        {
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();

            return;
        }

        Resource resource = myResource.GetComponent<Storage>().Resources.Items.First(); // TODO: Refactor.

        myGameObject.Orders.Pop();

        myGameObject.Transport(myResource, storage, resource.Name, resource.Current);
    }
}
