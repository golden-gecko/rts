public class unit_Infantry_Light_B_yup : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        MissilePrefab = "Objects/Missiles/Bullet";
        ReloadTimer = new Timer(1.0f);
        Speed = 2.0f;
    }
}
