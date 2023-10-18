using UnityEngine;

public class OrderHandlerAttackObject : OrderHandler
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

        if (order.TargetGameObject == null)
        {
            Success(myGameObject);

            return;
        }
        else
        {
            position = order.TargetGameObject.Center;
        }

        if (myGameObject.GetComponent<Engine>())
        {
            if (myGameObject.GetComponent<Gun>().IsInRange(position) == false)
            {
                myGameObject.Move(GetPositionToAttack(myGameObject.Position, position, myGameObject.GetComponent<Gun>().Range.Total), 0);
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
            if (myGameObject.GetComponent<Gun>().IsInRange(position))
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
        return order.TargetGameObject != null && order.TargetGameObject != myGameObject;
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
