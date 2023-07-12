using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerResearch : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (HasResources(myGameObject, order))
        {
            order.Timer.Update(Time.deltaTime);

            if (order.Timer.Finished)
            {
                if (HasResources(myGameObject, order))
                {
                    RemoveResources(myGameObject, order);

                    myGameObject.Player.TechnologyTree.Unlock(order.Technology);

                    myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                    myGameObject.Stats.Add(Stats.ResourcesUsed, 1);
                    myGameObject.Stats.Add(Stats.TechnologiesDiscovered, 1);
                }
                else
                {
                    myGameObject.Stats.Add(Stats.OrdersFailed, 1);
                }

                myGameObject.Orders.Pop();
            }
        }
    }

    private bool HasResources(MyGameObject myGameObject, Order order)
    {
        Dictionary<string, int> cost = myGameObject.Player.TechnologyTree.GetCost(order.Technology);

        foreach (KeyValuePair<string, int> i in cost)
        {
            if (myGameObject.Resources.Storage(i.Key) < i.Value)
            {
                return false;
            }
        }

        return true;
    }

    private void RemoveResources(MyGameObject myGameObject, Order order)
    {
        Dictionary<string, int> cost = myGameObject.Player.TechnologyTree.GetCost(order.Technology);

        foreach (KeyValuePair<string, int> i in cost)
        {
            myGameObject.Resources.Remove(i.Key, i.Value);
        }
    }
}
