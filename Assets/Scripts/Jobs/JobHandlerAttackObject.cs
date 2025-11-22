using UnityEngine;

public class JobHandlerAttackObject : JobHandler
{
    public override Order OnExecute(MyGameObject myGameObject)
    {
        if (myGameObject.Gun == null)
        {
            return null;
        }

        float minDistance = float.MaxValue;
        MyGameObject closest = null;

        foreach (MyGameObject target in Object.FindObjectsByType<MyGameObject>(FindObjectsSortMode.None))
        {
            if (myGameObject.Is(target, DiplomacyState.Enemy) == false)
            {
                continue;
            }

            if (myGameObject.Gun.IsInRange(target.Position) == false)
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

        if (closest == null)
        {
            return null;
        }

        return Order.AttackObject(closest);
    }
}
