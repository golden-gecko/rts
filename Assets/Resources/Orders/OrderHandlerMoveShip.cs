using UnityEngine;

public class OrderHandlerMoveShip : IOrderHandler
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

        target.y = 0;
        position.y = 0;

        float distanceToTarget = (target - position).magnitude;
        float distanceToTravel = myGameObject.GetComponent<Engine>().Speed * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            myGameObject.transform.LookAt(new Vector3(target.x, myGameObject.Position.y, target.z));
            myGameObject.transform.Translate(Vector3.forward * distanceToTravel);
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTravel);
        }
        else
        {
            myGameObject.Position = target;
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTarget);
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();
        }

        Vector3 newPosition = Map.Instance.VehiclePositionHandler.GetPosition(myGameObject.Position);

        newPosition.y += myGameObject.Altitude;

        myGameObject.Position = newPosition;
    }
}
