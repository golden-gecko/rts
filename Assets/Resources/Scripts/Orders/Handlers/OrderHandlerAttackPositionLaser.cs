using UnityEngine;

public class OrderHandlerAttackPositionLaser : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 direction = order.TargetPosition - myGameObject.Center;
        RaycastHit[] hits = Utils.RaycastAll(new Ray(myGameObject.Center, direction));

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
            if (Map.Instance.IsVisibleBySight(myGameObject.Position, HUD.Instance.ActivePlayer))
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

            Missile missile = myGameObject.GetComponent<Missile>();
            float damageDealt = closest.OnDamage(missile.Damage.Total);

            if (closest.Alive == false)
            {
                myGameObject.Stats.Inc(Stats.TargetsDestroyed);
            }

            if (Map.Instance.IsVisibleBySight(myGameObject.Position, HUD.Instance.ActivePlayer))
            {
                Object.Instantiate(myGameObject.GetComponent<Missile>().HitEffectPrefab, hitPoint, Quaternion.identity);
            }

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);

            myGameObject.Orders.Pop();
        }
    }
}
