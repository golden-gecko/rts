using UnityEngine;

public class OrderHandlerMove : OrderHandler
{
    public float GetDistance(MyGameObject myGameObject, Vector3 a, Vector3 b)
    {
        if (myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air))
        {
            if (myGameObject.Altitude > 0)
            {
                a.y = myGameObject.Altitude;
                b.y = myGameObject.Altitude;
            }
        }
        else
        {
            a.y = 0;
            b.y = 0;
        }

        return (a - b).magnitude;
    }

    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 target = order.TargetPosition;
        Vector3 position = myGameObject.Position;

        float distanceToTarget = GetDistance(myGameObject, position, target);
        float distanceToTravel = myGameObject.GetComponent<Engine>().Speed * Time.deltaTime;

        myGameObject.transform.LookAt(new Vector3(target.x, myGameObject.Position.y, target.z));

        if (distanceToTarget > distanceToTravel)
        {
            Vector3 validated;

            if (Map.Instance.ValidatePosition(myGameObject, myGameObject.Position + (target - position).normalized * distanceToTravel, out validated) == false)
            {
                Fail(myGameObject);

                return;
            }

            if (myGameObject.GetComponent<Engine>().CanDrive(distanceToTravel) == false)
            {
                myGameObject.Wait(0);

                return;
            }

            myGameObject.GetComponent<Engine>().Drive(distanceToTravel);
            myGameObject.OnMove(validated);
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTravel);
        }
        else
        {
            Vector3 validated;

            if (Map.Instance.ValidatePosition(myGameObject, target, out validated) == false)
            {
                Fail(myGameObject);

                return;
            }

            if (myGameObject.GetComponent<Engine>().CanDrive(distanceToTarget) == false)
            {
                myGameObject.Wait(0);

                return;
            }

            myGameObject.GetComponent<Engine>().Drive(distanceToTarget);
            myGameObject.OnMove(validated);
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTarget);
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();
        }
    }
}
