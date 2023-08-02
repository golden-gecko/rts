using UnityEngine;

public class OrderHandlerMoveMissile : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 target;
        Vector3 position = myGameObject.Position;

        if (order.TargetGameObject != null)
        {
            target = order.TargetGameObject.Entrance;
        }
        else
        {
            target = order.TargetPosition;
        }

        target.y = 0;
        position.y = 0;

        float distanceToTarget = (target - position).magnitude;
        float distanceToTravel = myGameObject.Speed * Time.deltaTime;

        if (distanceToTarget > distanceToTravel) // TODO: Add range to this order so units can stop when target is in their range.
        {
            myGameObject.transform.LookAt(new Vector3(target.x, myGameObject.transform.position.y, target.z));
            myGameObject.transform.Translate(Vector3.forward * distanceToTravel);
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTravel);
        }
        else
        {
            myGameObject.Position = target;
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTarget);
            myGameObject.Stats.Add(Stats.OrdersExecuted, 1);

            myGameObject.Orders.Pop();
        }
    }
}
