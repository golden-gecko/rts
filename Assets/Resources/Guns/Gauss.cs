using UnityEngine;

public class Gauss : Gun
{
    public Gauss(string name, float damage, float range, float reload) : base(name, damage, range, reload)
    {
    }

    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance);

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

            target.OnDamage(Damage);

            Object.Instantiate(Resources.Load("Effects/CFXR3 Hit Electric C (Air)"), target.Position, Quaternion.identity); // TODO: Move effect name to configuration.
        }

        Reload.Reset();
    }
}
