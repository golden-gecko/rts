using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Gauss : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance); // TODO: Create missile prefab.

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

            Instantiate(Resources.Load(HitEffectPrefab), target.Position, Quaternion.identity); 

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);
        }

        Instantiate(Resources.Load(HitEffectPrefab), position, Quaternion.identity);

        Reload.Reset();
    }
}
