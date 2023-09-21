using UnityEngine;

public class OrderHandlerAttack : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
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

        if (myGameObject.GetComponent<Engine>())
        {
            if (myGameObject.GetComponent<Gun>().IsInRange(position) == false)
            {
                myGameObject.Move(GetPositionToAttack(myGameObject.Position, position, myGameObject.GetComponent<Gun>().Range.Value), 0);
            }
            else
            {
                myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));

                if (myGameObject.GetComponent<Gun>().CanFire())
                {
                    myGameObject.GetComponent<Gun>().Fire(myGameObject, position);
                }
            }
        }
        else
        {
            if (myGameObject.GetComponent<Gun>().IsInRange(position) == false)
            {
                // TODO: Try to target to different object.
            }
            else
            {
                myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));

                if (myGameObject.GetComponent<Gun>().CanFire())
                {
                    myGameObject.GetComponent<Gun>().Fire(myGameObject, position);
                }
            }
        }
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.IsTargetGameObject == false || (order.IsTargetGameObject == true && order.TargetGameObject != null && order.TargetGameObject != myGameObject);
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
