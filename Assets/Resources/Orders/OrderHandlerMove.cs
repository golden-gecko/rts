using UnityEngine;

public class OrderHandlerMove : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 target = order.TargetPosition;
        Vector3 position = myGameObject.Position;

        target.y = 0;
        position.y = 0;

        float distanceToTarget = (target - position).magnitude;
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
            myGameObject.Position = validated;
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
            myGameObject.Position = validated;
            myGameObject.Stats.Add(Stats.DistanceDriven, distanceToTarget);
            myGameObject.Stats.Inc(Stats.OrdersCompleted);
            myGameObject.Orders.Pop();
        }
    }
}
