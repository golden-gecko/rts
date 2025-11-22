using UnityEngine;

public class Laser : Gun
{
    public override void Fire(MyGameObject myGameObject, Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(myGameObject.Center, position - myGameObject.Center), Config.RaycastMaxDistance); // TODO: Create missile prefab.

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
            float damageDealt = closest.OnDamage(Damage);
            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);

            Object.Instantiate(Resources.Load("Effects/CFXR3 Hit Electric C (Air)"), closest.Position, Quaternion.identity); // TODO: Move effect name to configuration.
        }

        Reload.Reset();
    }
}
