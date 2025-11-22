using UnityEngine;

public class OrderHandlerAttackUnit : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.IsTargetGameObject == false || (order.IsTargetGameObject == true && order.TargetGameObject != null);
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();

            return;
        }

        Vector3 position;

        if (order.IsTargetGameObject)
        {
            if (order.TargetGameObject == null)
            {
                myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
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
            myGameObject.Move(GetPositionToAttack(myGameObject.Position, position, myGameObject.GetComponent<Gun>().Range), 0);
        }
        else
        {
            myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));

            if (myGameObject.GetComponent<Gun>().Reload.Finished)
            {
                myGameObject.GetComponent<Gun>().Fire(myGameObject, position);
                myGameObject.Stats.Add(Stats.MissilesFired, 1);
                myGameObject.Orders.MoveToEnd();
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
