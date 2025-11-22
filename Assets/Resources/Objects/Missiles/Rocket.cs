using UnityEngine;

public class Rocket : Missile
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        MyGameObject myGameObject = other.GetComponentInParent<MyGameObject>();

        if (myGameObject == null)
        {
            return;
        }
            
        if (myGameObject.Is(this, DiplomacyState.Ally))
        {
            return;
        }

        if (myGameObject.GetComponent<Missile>())
        {
            return;
        }

        if (Map.Instance.IsVisibleBySight(myGameObject, HUD.Instance.ActivePlayer))
        {
            Instantiate(HitEffectPrefab, Position, Quaternion.identity);
        }

        float damageDealt = myGameObject.OnDamage(Damage.Total);

        if (myGameObject.Alive == false)
        {
            Parent.Stats.Inc(Stats.TargetsDestroyed);
        }

        Parent.Stats.Add(Stats.DamageDealt, damageDealt);

        Destroy(0);
    }

    // TODO: Add hit effect when missile does not hit anything.
}
