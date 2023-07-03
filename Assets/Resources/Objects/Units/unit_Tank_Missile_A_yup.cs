public class unit_Tank_Missile_A_yup : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 6.0f;
        MissilePrefab = "Objects/Missiles/Rocket";
        ReloadTimer = new Timer(5.0f);
    }
}
