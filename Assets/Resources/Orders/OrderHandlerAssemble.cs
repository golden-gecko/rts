using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerAssemble : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            order.TargetGameObject = Game.Instance.CreateGameObject(order.Prefab, myGameObject.Exit, myGameObject.Player, MyGameObjectState.UnderAssembly);
        }
        else if (order.TargetGameObject.Constructed == false)
        {
            MoveResourcesToUnit(myGameObject, order);
        }
        else if (order.TargetGameObject.Constructed)
        {
            order.Timer.Update(Time.deltaTime);

            if (order.Timer.Finished)
            {
                order.TargetGameObject.State = MyGameObjectState.Operational;
                order.TargetGameObject.Move(myGameObject.RallyPoint, 0);
                order.Timer.Reset();

                myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                myGameObject.Stats.Add(Stats.TimeConstructing, order.Timer.Max);
                myGameObject.Orders.Pop();
            }
        }
        else
        {
            myGameObject.Wait(0);
        }
    }

    private void MoveResourcesToUnit(MyGameObject myGameObject, Order order)
    {
        foreach (KeyValuePair<string, Resource> i in order.TargetGameObject.ConstructionResources.Items)
        {
            int capacity = i.Value.Capacity();
            int storage = myGameObject.Resources.Storage(i.Key);
            int value = Mathf.Min(new int[] { capacity, storage });

            if (value > 0)
            {
                myGameObject.Resources.Remove(i.Key, value);
                i.Value.Add(value);
            }
        }
    }
}
