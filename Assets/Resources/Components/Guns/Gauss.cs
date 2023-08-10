using UnityEngine;

public class Gauss : Gun
{
    public Gauss(MyGameObject parent, string name, float mass, float damage, float range, float reload) : base(parent, name, mass, damage, range, reload)
    {
    }

    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance); // TODO: Create missile prefab.

        foreach (RaycastHit i in hits)
        {
            MyGameObject target = i.transform.GetComponentInParent<MyGameObject>();

            if (target == null)
            {
                continue;
            }

            if (target.IsAlly(myGameObject))
            {
                continue;
            }

            if (target.GetComponent<Missile>())
            {
                continue;
            }

            float damageDealt = target.OnDamage(Damage);
            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);

            Object.Instantiate(Resources.Load("Effects/CFXR3 Hit Electric C (Air)"), target.Position, Quaternion.identity); // TODO: Move effect name to configuration.
        }

        Reload.Reset();
    }
}
