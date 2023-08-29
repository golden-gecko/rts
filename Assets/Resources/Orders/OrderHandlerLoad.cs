using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerLoad : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.SourceGameObject != null;
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

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        // Check storage and capacity.
        Dictionary<string, int> resources = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> i in order.Resources)
        {
            int value = Mathf.Min(new int[] { i.Value, order.SourceGameObject.GetComponent<Storage>().Resources.Storage(i.Key), myGameObject.GetComponent<Storage>().Resources.Capacity(i.Key) });

            if (value > 0)
            {
                resources[i.Key] = value;
            }
        }

        // Move resources or wait for them.
        if (resources.Count > 0)
        {
            myGameObject.MoveResources(order.SourceGameObject, myGameObject, resources);
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

                GameMenu.Instance.Log("Failed to execute load order");
            }
        }
    }
}
