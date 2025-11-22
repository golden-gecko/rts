using UnityEngine;

public class GaussMissile : Missile
{
    protected override void Start()
    {
        base.Start();

        transform.LookAt(Target);

        MyGameObject myGameObject = GetComponent<MyGameObject>();

        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, Target - myGameObject.Center), Config.RaycastMaxDistance);

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

        Instantiate(HitEffectPrefab, Target, Quaternion.identity);
    }
}
