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

            MyGameObject storage = GetStorage(myGameObject, order.TargetGameObject as MyResource); // TODO: Check type.

            GatherFromObject(myGameObject, order.TargetGameObject, storage);
        }
        else
        {
            MyResource myResource = GetResource(myGameObject);

            if (myResource == null)
            {
                Fail(myGameObject);

                return;
            }

            MyGameObject storage = GetStorage(myGameObject, myResource);

            if (storage == null)
            {
                if (myResource == null)
                {
                    Fail(myGameObject);

                    return;
                }
            }

            GatherFromObject(myGameObject, myResource, storage);
        }

        myGameObject.Orders.MoveToEnd();
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

        Resource resource = myResource.GetComponent<Storage>().Resources.Items.Values.First(); // TODO: Refactor.

        myGameObject.Transport(myResource, storage, resource.Name, resource.Storage);
    }

    private MyResource GetResource(MyGameObject myGameObject)
    {
        MyResource closest = null;
        float distance = float.MaxValue;

        foreach (MyResource myResource in Object.FindObjectsByType<MyResource>(FindObjectsSortMode.None))
        {
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
        MyResource closest = null;
        float distance = float.MaxValue;

        foreach (MyGameObject i in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None)) // TODO: Replace with Storage component.
        {
            if (i == myGameObject)
            {
                continue;
            }

            if (i == myResource)
            {
                continue;
            }

            if (i.GetComponent<Storage>() == null)
            {
                continue;
            }

            string[] storage = myResource.GetComponent<Storage>().Resources.Items.Keys.ToArray();
            string[] capacity = i.GetComponent<Storage>().Resources.Items.Keys.ToArray();

            foreach (string storageKey in storage)
            {
                if (capacity.Contains(storageKey) == false)
                {
                    continue;
                }

                float magnitude = (myGameObject.Position - myResource.Position).magnitude;

                if (magnitude < distance)
                {
                    closest = myResource;
                    distance = magnitude;
                }

                break;
            }
        }

        return closest;
    }
}
