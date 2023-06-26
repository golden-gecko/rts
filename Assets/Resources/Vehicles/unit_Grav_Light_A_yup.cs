public class unit_Grav_Light_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Health = 50.0f;
        MaxHealth = 50.0f;
        MissilePrefab = "Missiles/Rocket";
        MissileRangeMax = 30.0f;
        ReloadTimer = new Timer(2.0f);
        Speed = 8.0f;
    }
}
