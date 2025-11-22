using UnityEngine;

public class OrderHandlerMoveMissile: IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 target = order.TargetPosition;
        Vector3 position = myGameObject.Position;

        float distanceToTarget = (target - position).magnitude;
        float distanceToTravel = myGameObject.GetComponent<Engine>().Speed * Time.deltaTime;

        myGameObject.transform.LookAt(new Vector3(target.x, myGameObject.Position.y, target.z));

        if (distanceToTarget > distanceToTravel)
        {
            myGameObject.Position = myGameObject.Position + (target - position).normalized * distanceToTravel; // TODO: Check if missile is inside terrain.
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTravel);
        }
        else
        {
            myGameObject.Position = target;
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTarget); // TODO: Check if missile is inside terrain.
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();
        }
    }
}
