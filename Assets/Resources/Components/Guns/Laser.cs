using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance); // TODO: Create missile prefab.

        MyGameObject closest = null;
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
                distance = hit.distance;
            }
        }

        if (closest != null)
        {
            float damageDealt = closest.OnDamage(Damage);

            if (closest.Alive == false)
            {
                myGameObject.Stats.Inc(Stats.TargetsDestroyed);
            }

            Instantiate(Resources.Load(HitEffectPrefab), closest.Position, Quaternion.identity);

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);
        }
        else
        {
            Instantiate(Resources.Load(HitEffectPrefab), closest.Position, Quaternion.identity);
        }

        Reload.Reset();
    }
}
