using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Laser : Gun
{
    public Laser(string name, float damage, float range, float reload) : base(name, damage, range, reload)
    {
    }

    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance);

        MyGameObject closest = null;
        float distance = float.MaxValue;

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

            if (i.distance < distance)
            {
                closest = target;
                distance = i.distance;
            }
        }

        if (closest != null)
        {
            closest.OnDamage(Damage);

            Object.Instantiate(Resources.Load("Effects/CFXR3 Hit Electric C (Air)"), closest.Position, Quaternion.identity); // TODO: Move effect name to configuration.
        }

        Reload.Reset();
    }
}
