using UnityEngine;

public class OrderHandlerAttackLaser : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        Vector3 direction = order.TargetPosition - myGameObject.Center;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, direction), Config.RaycastMaxDistance);

        MyGameObject closest = null;
        Vector3 hitPoint = Vector3.zero;
        float distance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            MyGameObject target = hit.transform.GetComponentInParent<MyGameObject>();

            if (target == null)
            {
                continue;
            }

            if (target.Is(myGameObject, DiplomacyState.Ally))
            {
                continue;
            }

            if (target.GetComponent<Missile>())
            {
                continue;
            }

            if (hit.distance < distance)
            {
                closest = target;
                hitPoint = hit.point;
                distance = hit.distance;
            }
        }

        if (closest == null)
        {
            if (Map.Instance.IsVisibleBySight(myGameObject, HUD.Instance.ActivePlayer))
            {
                Object.Instantiate(myGameObject.GetComponent<Missile>().HitEffectPrefab, order.TargetPosition, Quaternion.identity);
            }
        }
        else
        {
            float magnitude = (closest.Position - myGameObject.Center).magnitude;

            myGameObject.Body.transform.localPosition = new Vector3(0.0f, 0.0f, magnitude / 2.0f);
            myGameObject.Body.transform.localScale = new Vector3(myGameObject.Body.transform.localScale.x, myGameObject.Body.transform.localScale.y, magnitude);
            myGameObject.transform.LookAt(order.TargetPosition);

            float damageDealt = closest.OnDamage(myGameObject.GetComponent<Missile>().Damage);

            if (closest.Alive == false)
            {
                myGameObject.Stats.Inc(Stats.TargetsDestroyed);
            }

            if (Map.Instance.IsVisibleBySight(myGameObject, HUD.Instance.ActivePlayer))
            {
                Object.Instantiate(myGameObject.GetComponent<Missile>().HitEffectPrefab, hitPoint, Quaternion.identity);
            }

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);

            myGameObject.Orders.Pop();
        }
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.IsTargetGameObject == false || (order.IsTargetGameObject == true && order.TargetGameObject != null && order.TargetGameObject != myGameObject);
    }
}
