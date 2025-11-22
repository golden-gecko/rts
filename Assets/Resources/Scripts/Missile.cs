using UnityEngine;

public class Missile : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Move);

        OrderHandlers[OrderType.Destroy] = new OrderHandlerDestroyMissile();
        OrderHandlers[OrderType.Move] = new OrderHandlerMoveMissile();

        Health = 1.0f;
        MaxHealth = 1.0f;
    }

    protected void OnTriggerEnter(Collider collider)
    {
        MyGameObject gameObject = collider.GetComponent<MyGameObject>();

        if (gameObject != null && gameObject != Parent && gameObject.Player != null && gameObject.Player != Player)
        {
            gameObject.OnDamage(Damage);

            Destroy(0);
        }
    }
}
