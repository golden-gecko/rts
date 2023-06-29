public class unit_Grav_Light_A_yup : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        MissilePrefab = "Missiles/Rocket";
        MissileRangeMax = 30.0f;
        ReloadTimer = new Timer(2.0f);
        Speed = 8.0f;
    }
}
