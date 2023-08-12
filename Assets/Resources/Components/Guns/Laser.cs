using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

            if (target.Is(myGameObject, DiplomacyState.Ally))
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

            if (closest.Alive)
            {
                Instantiate(Resources.Load(HitEffectPrefab), closest.Position, Quaternion.identity);
            }
            else
            {
                myGameObject.Stats.Add(Stats.TargetsDestroyed, 1); 
            }

            myGameObject.Stats.Add(Stats.DamageDealt, damageDealt);
        }

        Reload.Reset();
    }
}
