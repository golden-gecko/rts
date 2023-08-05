using UnityEngine;

public class OrderHandlerAttackUnit : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 position;

        if (order.IsTargetGameObject)
        {
            if (order.TargetGameObject == null)
            {
                myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                myGameObject.Stats.Add(Stats.TargetsDestroyed, 1);
                myGameObject.Orders.Pop();

                return;
            }
            else
            {
                position = order.TargetGameObject.Center;
            }
        }
        else
        {
            position = order.TargetPosition;
        }

        if (myGameObject.IsInAttackRange(position) == false)
        {
            myGameObject.Move(GetPositionToAttack(myGameObject.Position, position, myGameObject.MissileRange), 0);
        }
        else
        {
            myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));

            if (myGameObject.ReloadTimer.Finished)
            {
                MyGameObject resource = Resources.Load<MyGameObject>(myGameObject.MissilePrefab);
                MyGameObject missile = Object.Instantiate(resource, myGameObject.Center, Quaternion.identity);

                missile.Parent = myGameObject;
                missile.Player = myGameObject.Player;

                missile.Move(position);
                missile.Destroy();

                myGameObject.ReloadTimer.Reset();
                myGameObject.Orders.MoveToEnd();
                myGameObject.Stats.Add(Stats.MissilesFired, 1);
            }
        }
    }

    private Vector3 GetPositionToAttack(Vector3 position, Vector3 target, float missileRangeMax)
    {
        Vector3 a = position;
        Vector3 b = target;

        a.y = 0.0f;
        b.y = 0.0f;

        Vector3 direction = b - a;
        float magnitude = direction.magnitude;

        if (magnitude > missileRangeMax)
        {
            direction.Normalize();

            return position + direction;
        }

        return position;
    }
}
