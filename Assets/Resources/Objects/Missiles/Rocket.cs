using UnityEngine;

public class Rocket : Missile
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        MyGameObject myGameObject = other.GetComponentInParent<MyGameObject>(); // TODO: Add collision with terrain.

        if (myGameObject != null && myGameObject.Is(this, DiplomacyState.Ally) == false && myGameObject.GetComponent<Missile>() == false)
        {
            Instantiate(HitEffectPrefab, Position, Quaternion.identity);

            float damageDealt = myGameObject.OnDamage(Damage);

            if (myGameObject.Alive == false)
            {
                Parent.Stats.Inc(Stats.TargetsDestroyed);
            }

            Parent.Stats.Add(Stats.DamageDealt, damageDealt);

            Orders.Clear();

            Destroy();
        }
    }

    // TODO: Add hit effect when missile does not hit anything.
}
