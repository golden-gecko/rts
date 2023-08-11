using UnityEngine;

public class Missile : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Move);

        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroyMissile();
    }

    protected void OnTriggerEnter(Collider collider)
    {
        MyGameObject myGameObject = collider.GetComponentInParent<MyGameObject>(); // TODO: Add collision with terrain.

        if (myGameObject != null && myGameObject.IsAlly(this) == false && myGameObject.GetComponent<Missile>() == false)
        {
            float damageDealt = myGameObject.OnDamage(Damage);
            Parent.Stats.Add(Stats.DamageDealt, damageDealt);

            Orders.Clear();

            Destroy();
        }
    }

    public float Damage { get; protected set; } = 0.0f;
}
