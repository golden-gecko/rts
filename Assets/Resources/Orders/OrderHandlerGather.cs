using System.Linq;
using UnityEngine;

public class OrderHandlerGather : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
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

            MyGameObject storage = GetStorage(myGameObject, myResource);

            if (storage == null)
            {
                Fail(myGameObject);

                return;
            }

            GatherFromObject(myGameObject, order.TargetGameObject, storage);
        }
        else
        {
            MyResource myResource = GetResource(myGameObject, order.Resource);

            if (myResource == null)
            {
                Fail(myGameObject);

                return;
            }

            MyGameObject storage = GetStorage(myGameObject, myResource);

            if (storage == null)
            {
                Fail(myGameObject);

                return;
            }

            GatherFromObject(myGameObject, myResource, storage);
        }
    }

    protected override bool IsValid(Order order)
    {
        return order.IsTargetGameObject == true || order.Resource.Length > 0;
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

        myGameObject.Transport(myResource, storage, resource.Name, resource.Storage);
    }

    private MyResource GetResource(MyGameObject myGameObject, string resource = "")
    {
        MyResource closest = null;
        float distance = float.MaxValue;

        foreach (MyResource myResource in Object.FindObjectsByType<MyResource>(FindObjectsSortMode.None))
        {
            if (myResource.Working == false)
            {
                continue;
            }

            if (myResource == myGameObject)
            {
                continue;
            }

            if (myResource.Gatherable == false)
            {
                continue;
            }

            if (resource != "" && myResource.GetComponent<Storage>().Resources.Storage(resource) <= 0)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - myResource.Position).magnitude;

            if (magnitude < distance)
            {
                closest = myResource;
                distance = magnitude;
            }
        }

        return closest;
    }

    private MyGameObject GetStorage(MyGameObject myGameObject, MyResource myResource)
    {
        MyGameObject closest = null;
        float distance = float.MaxValue;

        foreach (Storage storage in Object.FindObjectsByType<Storage>(FindObjectsSortMode.None))
        {
            MyGameObject parent = storage.GetComponent<MyGameObject>();

            if (parent == null)
            {
                continue;
            }

            if (parent.Working == false)
            {
                continue;
            }

            if (parent == myGameObject)
            {
                continue;
            }

            if (parent == myResource)
            {
                continue;
            }

            string[] resourcesFromStorage = myResource.GetComponent<Storage>().Resources.Items.Where(x => x.Out && x.Empty == false).Select(x => x.Name).ToArray();
            string[] resourcesFromCapacity = storage.Resources.Items.Where(x => x.In && x.Full == false).Select(x => x.Name).ToArray();
            string[] match = resourcesFromStorage.Intersect(resourcesFromCapacity).ToArray();

            if (match.Length <= 0)
            {
                continue;
            }

            float magnitude = (myGameObject.Position - parent.Position).magnitude;

            if (magnitude < distance)
            {
                closest = parent;
                distance = magnitude;
            }
        }

        return closest;
    }
}
