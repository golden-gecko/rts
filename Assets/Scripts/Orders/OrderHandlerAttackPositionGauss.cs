using UnityEngine;

public class OrderHandlerAttackPositionGauss : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        Vector3 direction = order.TargetPosition - myGameObject.Center;
        RaycastHit[] hits = Utils.RaycastAll(new Ray(myGameObject.Center, direction));

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

            Missile missile = myGameObject.GetComponent<Missile>();
            float damageDealt = target.OnDamageHandler(missile.DamageType, missile.Damage.Total);

            if (target.Alive == false)
            {
                myGameObject.Stats.Inc(Stats.TargetsDestroyed);
            }

            if (Map.Instance.IsVisibleBySight(hit.point, HUD.Instance.ActivePlayer))
            {
                Object.Instantiate(myGameObject.GetComponent<Missile>().HitEffectPrefab, hit.point, Quaternion.identity);
            }

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);
        }

        if (Map.Instance.IsVisibleBySight(order.TargetPosition, HUD.Instance.ActivePlayer))
        {
            Object.Instantiate(myGameObject.GetComponent<Missile>().HitEffectPrefab, order.TargetPosition, Quaternion.identity);
        }

        float range = myGameObject.GetComponent<Missile>().Range.Total;

        myGameObject.Body.transform.localPosition = new Vector3(0.0f, 0.0f, range / 2.0f);
        myGameObject.Body.transform.localScale = new Vector3(myGameObject.Body.transform.localScale.x, myGameObject.Body.transform.localScale.y, range);


        if (myGameObject.RotateTowardsTarget)
        {
            myGameObject.transform.LookAt(order.TargetPosition);
        }

        myGameObject.Orders.Pop();
    }
}
