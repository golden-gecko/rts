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

    // TODO: Add collision with terrain.
    protected void OnTriggerEnter(Collider collider)
    {
        MyGameObject gameObject = collider.GetComponent<MyGameObject>();

        if (gameObject != null && gameObject.IsAlly(this) == false)
        {
            gameObject.OnDamage(Damage);

            Destroy(0);
        }
    }

    // TODO: Disable this behaviour per object and fire missile from object center.
    protected override void AlignPositionToTerrain()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position + new Vector3(0, 1000, 0), Vector3.down);

        if (Physics.Raycast(ray, out hitInfo, 2000, LayerMask.GetMask("Terrain")))
        {
            if (hitInfo.transform.tag == "Terrain")
            {
                Position = new Vector3(transform.position.x, hitInfo.point.y + 0.1f, transform.position.z);
            }
        }
    }
}
