using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerConstruct : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        switch (order.PrefabConstructionType)
        {
            case PrefabConstructionType.Structure:
                if (myGameObject.IsCloseTo(order.TargetPosition + new Vector3(0, 0, 1)) == false)
                {
                    myGameObject.Move(order.TargetPosition + new Vector3(0, 0, 1), 0); // TODO: Add offset based on object size.
                }
                else if (order.TargetGameObject == null)
                {
                    MyGameObject resource = UnityEngine.Resources.Load<MyGameObject>(order.Prefab); // TODO: Remove name conflict.

                    order.TargetGameObject = Object.Instantiate<MyGameObject>(resource, order.TargetPosition, Quaternion.identity);
                    order.TargetGameObject.State = MyGameObjectState.UnderConstruction;
                }
                else if (order.TargetGameObject.IsConstructed())
                {
                    order.Timer.Update(Time.deltaTime);

                    if (order.Timer.Finished)
                    {
                        order.TargetGameObject.State = MyGameObjectState.Operational;
                        order.Timer.Reset();

                        myGameObject.Orders.Pop();

                        myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                        myGameObject.Stats.Add(Stats.TimeConstructing, order.Timer.Max);
                    }
                }
                else
                {
                    myGameObject.Wait(0);
                }

                break;

            case PrefabConstructionType.Unit:
                if (order.TargetGameObject == null)
                {
                    MyGameObject resource = Resources.Load<MyGameObject>(order.Prefab); // TODO: Remove name conflict.

                    order.TargetGameObject = Object.Instantiate<MyGameObject>(resource, myGameObject.Exit, Quaternion.identity);
                    order.TargetGameObject.State = MyGameObjectState.UnderAssembly;
                }
                else if (order.TargetGameObject.IsConstructed() == false)
                {
                    MoveResourcesToUnit(myGameObject, order);
                }
                else if (order.TargetGameObject.IsConstructed())
                {
                    order.Timer.Update(Time.deltaTime);

                    if (order.Timer.Finished)
                    {
                        order.TargetGameObject.State = MyGameObjectState.Operational;
                        order.TargetGameObject.Move(myGameObject.RallyPoint);
                        order.Timer.Reset();

                        myGameObject.Orders.Pop();

                        myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                        myGameObject.Stats.Add(Stats.TimeConstructing, order.Timer.Max);
                    }
                }
                else
                {
                    myGameObject.Wait(0);
                }

                break;
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
