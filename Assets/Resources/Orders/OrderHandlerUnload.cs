using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerUnload : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        // Update timer.
        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished == false)
        {
            return;
        }

        // Check storage and capacity.
        Dictionary<string, int> resources = new Dictionary<string, int>();

        foreach (KeyValuePair<string, int> i in order.Resources)
        {
            if (order.TargetGameObject.State == MyGameObjectState.UnderConstruction)
            {
                int value = Mathf.Min(new int[] { i.Value, myGameObject.Resources.Storage(i.Key), order.TargetGameObject.ConstructionResources.Capacity(i.Key) });

                if (value > 0)
                {
                    resources[i.Key] = value;

                    myGameObject.Stats.Add(Stats.ResourcesTransported, value);
                }
            }
            else
            {
                int value = Mathf.Min(new int[] { i.Value, myGameObject.Resources.Storage(i.Key), order.TargetGameObject.Resources.Capacity(i.Key) });

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
            myGameObject.Orders.Pop();
            myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
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
                myGameObject.Orders.Pop();
                myGameObject.Stats.Add(Stats.OrdersFailed, 1);

                GameMenu.Instance.Log("Failed to execute unload order");
            }
        }
    }
}
