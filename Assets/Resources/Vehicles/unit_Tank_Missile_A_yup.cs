public class unit_Tank_Missile_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 6.0f;
        MissilePrefab = "Missiles/Missile";
        MissileRangeMax = 30.0f;
        ReloadTimer = new Timer(5.0f);
        Health = 50.0f;
        MaxHealth = 50.0f;
    }
}
