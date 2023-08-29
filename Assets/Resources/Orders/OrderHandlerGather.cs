using System.Linq;
using UnityEngine;

public class OrderHandlerGather : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.IsTargetGameObject == true || order.Resource.Length > 0;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        if (order.IsTargetGameObject)
        {
            if (order.TargetGameObject == null)
            {
                myGameObject.Stats.Inc(Stats.OrdersCompleted);
                myGameObject.Orders.Pop();

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
                myGameObject.Stats.Inc(Stats.OrdersFailed);
                myGameObject.Orders.Pop();

                return;
            }

            MyGameObject storage = GetStorage(myGameObject, myResource);

            if (storage == null)
            {
                if (myResource == null)
                {
                    myGameObject.Stats.Inc(Stats.OrdersFailed);
                    myGameObject.Orders.Pop();

                    return;
                }
            }

            GatherFromObject(myGameObject, myResource, storage);
        }

        myGameObject.Orders.MoveToEnd();
    }

    private void GatherFromObject(MyGameObject myGameObject, MyGameObject myResource, MyGameObject storage)
    {
        if (myResource == null || storage == null)
        {
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();

            return;
        }

        myGameObject.Transport(myResource, storage, myResource.GetComponent<Storage>().Resources.GetStorage()); // TODO: Refactor.
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
