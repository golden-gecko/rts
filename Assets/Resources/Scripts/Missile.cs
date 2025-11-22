using UnityEngine;

public class Missile : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Move);

        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroyMissile();

        Health = 1.0f;
        MaxHealth = 1.0f;
    }

    // TODO: Add collision with terrain.
    protected void OnTriggerEnter(Collider collider)
    {
        MyGameObject myGameObject = collider.GetComponentInParent<MyGameObject>();

        if (myGameObject != null && myGameObject.IsAlly(this) == false && myGameObject.GetComponent<Missile>() == false)
        {
            myGameObject.OnDamage(Damage);

            Orders.Clear();

            Destroy();
        }
    }

    public float Damage { get; protected set; } = 0.0f;
}
