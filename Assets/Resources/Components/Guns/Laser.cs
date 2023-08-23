using UnityEngine;

public class Laser : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        // TODO: Create missile prefab.
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance);

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
            Instantiate(Resources.Load(HitEffectPrefab), position, Quaternion.identity);
        }
        else
        {
            float damageDealt = closest.OnDamage(Damage);

            if (closest.Alive == false)
            {
                myGameObject.Stats.Inc(Stats.TargetsDestroyed);
            }

            Instantiate(Resources.Load(HitEffectPrefab), hitPoint, Quaternion.identity);

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);
        }

        Reload.Reset();
    }
}
