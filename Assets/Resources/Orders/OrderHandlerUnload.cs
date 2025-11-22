using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerUnload : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.TargetGameObject != null;
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

        if (order.Timer.Update(Time.deltaTime)  == false)
        {
            return;
        }

        // Check storage and capacity.
        Dictionary<string, int> resources = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> i in order.Resources)
        {
            if (order.TargetGameObject.State == MyGameObjectState.UnderConstruction)
            {
                int value = Mathf.Min(new int[] { i.Value, myGameObject.GetComponent<Storage>().Resources.Storage(i.Key), order.TargetGameObject.ConstructionResources.Capacity(i.Key) });

                if (value > 0)
                {
                    resources[i.Key] = value;

                    myGameObject.Stats.Add(Stats.ResourcesTransported, value);
                }
            }
            else
            {
                int value = Mathf.Min(new int[] { i.Value, myGameObject.GetComponent<Storage>().Resources.Storage(i.Key), order.TargetGameObject.GetComponent<Storage>().Resources.Capacity(i.Key) });

                if (value > 0)
                {
                    resources[i.Key] = value;

                    myGameObject.Stats.Add(Stats.ResourcesTransported, value);
                }
            }
        }

        // Move resources or wait for them.
        if (resources.Count > 0)
        {
            myGameObject.MoveResources(myGameObject, order.TargetGameObject, resources);
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();
        }
        else
        {
            order.Retry();
            order.Timer.Reset();

            if (order.CanRetry)
            {
                myGameObject.Wait(0);
            }
            else
            {
                myGameObject.Stats.Inc(Stats.OrdersFailed);
                myGameObject.Orders.Pop();

                GameMenu.Instance.Log("Failed to execute unload order");
            }
        }
    }
}
