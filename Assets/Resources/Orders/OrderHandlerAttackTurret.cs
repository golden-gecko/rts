using UnityEngine;

public class OrderHandlerAttackTurret : IOrderHandler
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

        if (myGameObject.IsInRange(position, myGameObject.MissileRange))
        {
            myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));

            if (myGameObject.ReloadTimer.Finished)
            {
                MyGameObject resource = Resources.Load<MyGameObject>(myGameObject.MissilePrefab);
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
}
