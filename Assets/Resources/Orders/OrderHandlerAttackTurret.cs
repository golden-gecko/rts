using UnityEngine;

public class OrderHandlerAttackTurret : IOrderHandler
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
                myGameObject.Stats.Add(Stats.TargetsDestroyed, 1); // TODO: Move to missile.
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

        if (myGameObject.IsInAttackRange(position))
        {
            myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));

            if (myGameObject.GetComponent<Gun>().Reload.Finished)
            {
                myGameObject.GetComponent<Gun>().Fire(myGameObject, position);
                myGameObject.Orders.MoveToEnd();
                myGameObject.Stats.Add(Stats.MissilesFired, 1);
            }
        }
    }
}
