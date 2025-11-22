using UnityEngine;

public class OrderHandlerLoad : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (Utils.IsCloseTo(myGameObject.Position, order.SourceGameObject.Exit) == false)
        {
            myGameObject.Move(order.SourceGameObject.Exit, 0);

            return;
        }

        int valueStart = Mathf.Min(new int[]
            {
                order.Value,
                order.SourceGameObject.GetComponent<Storage>().Resources.Current(order.Resource),
                myGameObject.GetComponent<Storage>().Resources.Available(order.Resource)
            }
        );

        if (valueStart <= 0)
        {
            Fail(myGameObject);

            return;
        }

        if (order.Timer == null)
        {
            order.Timer = new Timer(Mathf.Ceil((float)valueStart / myGameObject.GetComponent<Storage>().ResourceUsage));
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        int valueEnd = Mathf.Min(new int[]
            {
                order.Value,
                order.SourceGameObject.GetComponent<Storage>().Resources.Current(order.Resource),
                myGameObject.GetComponent<Storage>().Resources.Available(order.Resource)
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
