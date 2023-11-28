using UnityEngine;

public class JobHandlerAttackObject : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        float minDistance = float.MaxValue;
        MyGameObject closest = null;

        foreach (MyGameObject target in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myGameObject.Is(target, DiplomacyState.Enemy) == false)
            {
                continue;
            }

            Gun gun = myGameObject.GetComponentInChildren<Gun>();

            if (gun == null)
            {
                continue;
            }

            if (gun.IsInRange(target.Position) == false)
            {
                continue;
            }

            if (target.GetComponent<Missile>())
            {
                continue;
            }

            float distance = myGameObject.DistanceTo(target);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }

        if (closest != null)
        {
            return Order.AttackObject(closest);
        }

        return null;
    }
}
