using UnityEngine;

public class Gauss : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        // TODO: Create missile prefab.
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance);

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

            float damageDealt = target.OnDamage(Damage);

            if (target.Alive == false)
            {
                myGameObject.Stats.Inc(Stats.TargetsDestroyed);
            }

            Instantiate(HitEffectPrefab, hit.point, Quaternion.identity); 

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);
        }

        Instantiate(HitEffectPrefab, position, Quaternion.identity);

        Reload.Reset();
    }
}
