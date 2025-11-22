using Unity.VisualScripting;
using UnityEngine;

public class OrderHandlerUnload : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (Utils.IsInRange(myGameObject.Position, order.TargetGameObject.Entrance, order.Range) == false)
        {
            myGameObject.Move(order.TargetGameObject.Entrance, 0);

            return;
        }

        int valueStart;

        if (order.TargetGameObject.State == MyGameObjectState.UnderConstruction)
        {
            int[] values = new int[]
            {
                order.Value,
                order.TargetGameObject.ConstructionResources.Available(order.Resource),
                myGameObject.GetComponentInChildren<Storage>().Resources.Current(order.Resource),
            };

            valueStart = Mathf.Min(values);
        }
        else
        {
            int[] values = new int[]
            {
                order.Value,
                order.TargetGameObject.GetComponentInChildren<Storage>().Resources.Available(order.Resource),
                myGameObject.GetComponentInChildren<Storage>().Resources.Current(order.Resource)
            };

            valueStart = Mathf.Min(values);
        }

        if (valueStart <= 0)
        {
            Fail(myGameObject);

            return;
        }

        if (order.Timer == null)
        {
            order.Timer = new Timer(Mathf.Ceil((float)valueStart / (float)myGameObject.GetComponentInChildren<Storage>().ResourceUsage));
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        int valueEnd;

        if (order.TargetGameObject.State == MyGameObjectState.UnderConstruction)
        {
            int[] values = new int[]
            {
                order.Value,
                order.TargetGameObject.ConstructionResources.Available(order.Resource),
                myGameObject.GetComponentInChildren<Storage>().Resources.Current(order.Resource),
            };

            valueEnd = Mathf.Min(values);
        }
        else
        {
            int[] values = new int[]
            {
                order.Value,
                order.TargetGameObject.GetComponentInChildren<Storage>().Resources.Available(order.Resource),
                myGameObject.GetComponentInChildren<Storage>().Resources.Current(order.Resource)
            };

            valueEnd = Mathf.Min(values);
        }

        if (valueStart != valueEnd)
        {
            Fail(myGameObject);

            return;
        }

        MoveResources(myGameObject, order.TargetGameObject, order.Resource, valueEnd);

        Success(myGameObject);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.TargetGameObject != null && order.TargetGameObject != myGameObject;
    }

    private void MoveResources(MyGameObject source, MyGameObject target, string resource, int value)
    {
        int added;

        if (target.State == MyGameObjectState.UnderConstruction)
        {
            added = target.ConstructionResources.Add(resource, value);
        }
        else
        {
            added = target.GetComponentInChildren<Storage>().Resources.Add(resource, value);
        }

        if (added > 0)
        {
            source.GetComponentInChildren<Storage>().Resources.Remove(resource, added);
        }
    }
}
