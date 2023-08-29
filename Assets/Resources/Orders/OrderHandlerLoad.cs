using UnityEngine;

public class OrderHandlerLoad : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            Fail(myGameObject);

            return;
        }

        int valueStart = Mathf.Min(new int[]
            {
                order.Value,
                order.SourceGameObject.GetComponent<Storage>().Resources.Storage(order.Resource),
                myGameObject.GetComponent<Storage>().Resources.Capacity(order.Resource)
            }
        );

        if (valueStart <= 0)
        {
            Fail(myGameObject);

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
                order.SourceGameObject.GetComponent<Storage>().Resources.Storage(order.Resource),
                myGameObject.GetComponent<Storage>().Resources.Capacity(order.Resource)
            }
        );

        if (valueStart != valueEnd)
        {
            Fail(myGameObject);

            return;
        }

        MoveResources(order.SourceGameObject, myGameObject, order.Resource, valueEnd);

        myGameObject.Stats.Inc(Stats.OrdersCompleted);
        myGameObject.Orders.Pop();
    }

    protected override bool IsValid(Order order)
    {
        return order.SourceGameObject != null;
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
