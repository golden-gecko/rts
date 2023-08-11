using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerLoad : IOrderHandler
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
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        // Check storage and capacity.
        Dictionary<string, int> resources = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> i in order.Resources)
        {
            int value = Mathf.Min(new int[] { i.Value, order.SourceGameObject.Resources.Storage(i.Key), myGameObject.Resources.Capacity(i.Key) });

            if (value > 0)
            {
                resources[i.Key] = value;
            }
        }

        // Move resources or wait for them.
        if (resources.Count > 0)
        {
            myGameObject.MoveResources(order.SourceGameObject, myGameObject, resources);
            myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
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
                myGameObject.Stats.Add(Stats.OrdersFailed, 1);
                myGameObject.Orders.Pop();

                GameMenu.Instance.Log("Failed to execute load order");
            }
        }
    }
}
