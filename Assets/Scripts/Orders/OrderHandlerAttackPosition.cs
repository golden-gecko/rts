using UnityEngine;

public class OrderHandlerAttackPosition : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 position = order.TargetPosition;

        if (myGameObject.GetComponentInChildren<Engine>())
        {
            if (myGameObject.GetComponentInChildren<Gun>().IsInRange(position) == false)
            {
                myGameObject.Move(GetPositionToAttack(myGameObject.Position, position, myGameObject.GetComponentInChildren<Gun>().Range.Total), 0);
            }
            else
            {
                if (myGameObject.RotateTowardsTarget)
                {
                    myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));
                }

                if (myGameObject.GetComponentInChildren<Gun>().CanFire())
                {
                    myGameObject.GetComponentInChildren<Gun>().Fire(myGameObject, position);
                }
            }
        }
        else
        {
            if (myGameObject.GetComponentInChildren<Gun>().IsInRange(position))
            {
                if (myGameObject.RotateTowardsTarget)
                {
                    myGameObject.transform.LookAt(new Vector3(position.x, myGameObject.Position.y, position.z));
                }

                if (myGameObject.GetComponentInChildren<Gun>().CanFire())
                {
                    myGameObject.GetComponentInChildren<Gun>().Fire(myGameObject, position);
                }
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
