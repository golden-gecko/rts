using UnityEngine;

public class Missile : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Move);

        Damage = 10;
        Speed = 10;
        MissilePrefab = "Prefabs/Missiles/Missile";
        MissileRange = 100;
        ReloadTime = 3;
    }

    protected override void OnOrderMove()
    {
        var order = Orders.First();

        Vector3 target;
        Vector3 position = transform.position;

        if (order.TargetGameObject != null)
        {
            target = order.TargetGameObject.Entrance;
        }
        else
        {
            target = order.TargetPosition;
        }

        var distanceToTarget = (target - position).magnitude;
        var distanceToTravel = Speed * Time.deltaTime;

        if (distanceToTarget > distanceToTravel)
        {
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
            transform.Translate(Vector3.forward * distanceToTravel);

            Stats.Add(Stats.DistanceDriven, distanceToTravel);
        }
        else
        {
            transform.position = target;

            Stats.Add(Stats.DistanceDriven, distanceToTarget);
            Stats.Add(Stats.OrdersExecuted, 1);

            Orders.Pop();
        }
    }
}
