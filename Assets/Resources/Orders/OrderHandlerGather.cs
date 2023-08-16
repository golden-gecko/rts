using UnityEngine;

public class OrderHandlerGather : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.IsTargetGameObject == true || order.Resource.Length > 0;
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

        if (order.IsTargetGameObject)
        {
            if (order.TargetGameObject == null)
            {
                myGameObject.Stats.Inc(Stats.OrdersCompleted);
                myGameObject.Orders.Pop();

                return;
            }

            GatherFromObject(order.TargetGameObject);
        }
        else
        {
            MyResource closest = null;
            float distance = float.MaxValue;

            foreach (MyResource myResource in GameObject.FindObjectsByType<MyResource>(FindObjectsSortMode.None))
            {
                float magnitude = (myGameObject.Position - myResource.Position).magnitude;

                if (magnitude < distance)
                {
                    closest = myResource;
                    distance = magnitude;
                }
            }

            if (closest != null)
            {
                GatherFromObject(closest);
            }
        }

        myGameObject.Orders.MoveToEnd();
    }

    private void GatherFromObject(MyGameObject myGameObject)
    {
        if (myGameObject == null)
        {
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();

            return;
        }

        if (myGameObject.IsCloseTo(myGameObject.Position) == false)
        {
            myGameObject.Move(myGameObject.Entrance, 0);

            return;
        }
    }
}
