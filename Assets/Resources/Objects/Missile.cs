using UnityEngine;

public class Missile : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Move);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        MyGameObject myGameObject = other.GetComponentInParent<MyGameObject>(); // TODO: Add collision with terrain.

        if (myGameObject != null && myGameObject.Is(this, DiplomacyState.Ally) == false && myGameObject.GetComponent<Missile>() == false)
        {
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

    [field: SerializeField]
    public float Damage { get; set; } = 10.0f;
}
