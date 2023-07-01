public class unit_Tank_Combat_A_yup : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 6.0f;
        MissilePrefab = "Missiles/Rocket";
        ReloadTimer = new Timer(5.0f);
    }
}
