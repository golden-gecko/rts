using UnityEngine;

public class OrderHandlerAttack : IOrderHandler
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
                position = order.TargetGameObject.Position;
            }
        }
        else
        {
            position = order.TargetPosition;
        }

        if (myGameObject.IsCloseTo(position, myGameObject.MissileRangeMax) == false) // TODO: Test by adding visual debug.
        {
            myGameObject.Move(GetPositionToAttack(myGameObject.Position, position, myGameObject.MissileRangeMax), 0); // TODO: Test by adding visual debug.
        }
        else
        {
            myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));

            if (myGameObject.ReloadTimer.Finished)
            {
                MyGameObject resource = Resources.Load<MyGameObject>(myGameObject.MissilePrefab); // TODO: Remove name conflict.
                MyGameObject missile = Object.Instantiate<MyGameObject>(resource, myGameObject.Position, Quaternion.identity);

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
        Vector3 direction = target - position;

        direction.Normalize();

        return target - direction * missileRangeMax * 0.9f;
    }
}
