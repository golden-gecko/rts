using System.Collections.Generic;
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

            MyGameObject storage = GetStorage(myGameObject);

            GatherFromObject(myGameObject, order.TargetGameObject, storage);
        }
        else
        {
            MyResource myResource = GetResource(myGameObject);
            MyGameObject storage = GetStorage(myGameObject);

            if (myResource != null && storage != null)
            {
                GatherFromObject(myGameObject, myResource, storage);
            }
        }

        myGameObject.Orders.MoveToEnd();
    }

    private MyResource GetResource(MyGameObject myGameObject)
    {
        return null;
    }

    private MyGameObject GetStorage(MyGameObject myGameObject)
    {
        MyResource closest = null;
        float distance = float.MaxValue;

        foreach (MyResource myResource in GameObject.FindObjectsByType<MyResource>(FindObjectsSortMode.None))
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

    private void GatherFromObject(MyGameObject myGameObject, MyGameObject myResource, MyGameObject storage)
    {
        if (myResource == null || storage == null)
        {
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();

            return;
        }

        myGameObject.Transport(myResource, storage, myResource.Resources.GetCapacity(), 0);
    }
}
