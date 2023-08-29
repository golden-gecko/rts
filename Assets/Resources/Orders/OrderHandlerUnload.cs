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
            myGameObject.Stats.Inc(Stats.OrdersFailed); // TODO: Create Fail method.
            myGameObject.Orders.Pop();

            return;
        }

        int valueStart = Mathf.Min(new int[]
            {
                order.Value,
                order.TargetGameObject.GetComponent<Storage>().Resources.Capacity(order.Resource),
                myGameObject.GetComponent<Storage>().Resources.Storage(order.Resource)
            }
        );

        if (valueStart <= 0)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        if (order.Timer == null)
        {
            order.Timer = new Timer(valueStart / myGameObject.GetComponent<Storage>().ResourceUsage);
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        int valueEnd = Mathf.Min(new int[]
            {
                order.Value,
                order.TargetGameObject.GetComponent<Storage>().Resources.Capacity(order.Resource),
                myGameObject.GetComponent<Storage>().Resources.Storage(order.Resource)
            }
        );

        if (valueStart != valueEnd)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        MoveResources(myGameObject, order.TargetGameObject, order.Resource, valueEnd);

        myGameObject.Stats.Inc(Stats.OrdersCompleted);
        myGameObject.Orders.Pop();
    }

    private void MoveResources(MyGameObject source, MyGameObject target, string resource, int value)
    {
        source.GetComponent<Storage>().Resources.Remove(resource, value);

        if (target.State == MyGameObjectState.UnderConstruction)
        {
            target.ConstructionResources.Add(resource, value);
        }
        else
        {
            target.GetComponent<Storage>().Resources.Add(resource, value);
        }
    }
}
