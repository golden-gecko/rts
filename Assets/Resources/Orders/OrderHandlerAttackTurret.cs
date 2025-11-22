using UnityEngine;

public class OrderHandlerAttackTurret : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            Fail(myGameObject);

            return;
        }

        Vector3 position;

        if (order.IsTargetGameObject)
        {
            if (order.TargetGameObject == null)
            {
                myGameObject.Stats.Inc(Stats.OrdersCompleted);
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
            }
        }
    }
    protected override bool IsValid(Order order)
    {
        return order.IsTargetGameObject == false || (order.IsTargetGameObject == true && order.TargetGameObject != null);
    }
}
