public class unit_Trike_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 12.0f;
        MissilePrefab = "Missiles/Missile";
        MissileRangeMax = 4.0f;
        ReloadTimer = new Timer(2.0f);
        Health = 50.0f;
        MaxHealth = 50.0f;
    }
}
