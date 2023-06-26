public class unit_Infantry_Light_B_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Health = 50.0f;
        MaxHealth = 50.0f;
        MissilePrefab = "Missiles/Bullet";
        MissileRangeMax = 5.0f;
        ReloadTimer = new Timer(1.0f);
        Speed = 2.0f;
    }
}
