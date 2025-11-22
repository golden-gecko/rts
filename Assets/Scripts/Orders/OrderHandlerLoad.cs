using UnityEngine;

public class OrderHandlerLoad : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (Utils.IsInRange(myGameObject.Position, order.SourceGameObject.Exit, order.Range) == false)
        {
            myGameObject.Move(order.SourceGameObject.Exit, 0);

            return;
        }

        int valueStart = Mathf.Min(new int[]
            {
                order.Value,
                order.SourceGameObject.GetComponentInChildren<Storage>().Resources.Current(order.Resource),
                myGameObject.GetComponentInChildren<Storage>().Resources.Available(order.Resource),
                10, // TODO: Hardcoded.
            }
        );

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

        int valueEnd = Mathf.Min(new int[]
            {
                order.Value,
                order.SourceGameObject.GetComponentInChildren<Storage>().Resources.Current(order.Resource),
                myGameObject.GetComponentInChildren<Storage>().Resources.Available(order.Resource),
                10, // TODO: Hardcoded.
            }
        );

        if (valueStart != valueEnd)
        {
            Fail(myGameObject);

            return;
        }

        MoveResources(order.SourceGameObject, myGameObject, order.Resource, valueEnd);

        Success(myGameObject);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.SourceGameObject != null && order.SourceGameObject != myGameObject;
    }

    private void MoveResources(MyGameObject source, MyGameObject target, string resource, int value)
    {
        source.GetComponentInChildren<Storage>().Resources.Remove(resource, value);

        if (target.State == MyGameObjectState.UnderConstruction)
        {
            target.ConstructionResources.Add(resource, value);
        }
        else
        {
            target.GetComponentInChildren<Storage>().Resources.Add(resource, value);
        }
    }
}
