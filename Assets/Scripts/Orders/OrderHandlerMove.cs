using UnityEngine;

public class OrderHandlerMove : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 target = order.TargetPosition;
        Vector3 position = myGameObject.Position;

        float distanceToTarget = GetDistance(myGameObject, ref position, ref target);
        float distanceToTravel = myGameObject.GetComponentInChildren<Engine>().Speed * Time.deltaTime;

        if (myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air))
        {
            myGameObject.transform.LookAt(target);
        }
        else
        {
            myGameObject.transform.LookAt(new Vector3(target.x, myGameObject.Position.y, target.z));
        }

        if (distanceToTarget > distanceToTravel)
        {
            Vector3 validated;

            if (Map.Instance.ValidatePosition(myGameObject, myGameObject.Position + (target - position).normalized * distanceToTravel, out validated) == false)
            {
                Fail(myGameObject);

                return;
            }

            if (myGameObject.GetComponentInChildren<Engine>().CanDrive(distanceToTravel) == false)
            {
                myGameObject.Wait(0);

                return;
            }

            myGameObject.GetComponentInChildren<Engine>().Drive(distanceToTravel);
            myGameObject.OnMoveHandler(validated);
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

            if (myGameObject.GetComponentInChildren<Engine>().CanDrive(distanceToTarget) == false)
            {
                myGameObject.Wait(0);

                return;
            }

            myGameObject.GetComponentInChildren<Engine>().Drive(distanceToTarget);
            myGameObject.OnMoveHandler(validated);
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTarget);

            Success(myGameObject);
        }
    }

    private float GetDistance(MyGameObject myGameObject, ref Vector3 a, ref Vector3 b)
    {
        if (myGameObject.MapLayers.Contains(MyGameObjectMapLayer.Air) == false)
        {
            a.y = 0.0f;
            b.y = 0.0f;
        }

        return (a - b).magnitude;
    }
}
