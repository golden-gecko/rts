public class unit_Trike_A_yup : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 12.0f;
        MissilePrefab = "Missiles/Bullet";
        MissileRangeMax = 4.0f;
        ReloadTimer = new Timer(2.0f);
    }
}
