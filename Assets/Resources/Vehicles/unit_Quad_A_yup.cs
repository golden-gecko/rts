public class unit_Quad_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 12.0f;
        MissilePrefab = "Missiles/Bullet";
        MissileRangeMax = 20.0f;
        ReloadTimer = new Timer(3.0f);
        Health = 50.0f;
        MaxHealth = 50.0f;
    }
}
