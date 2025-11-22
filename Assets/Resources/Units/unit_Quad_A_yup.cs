public class unit_Quad_A_yup : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 12.0f;
        MissilePrefab = "Missiles/Bullet";
        ReloadTimer = new Timer(3.0f);
    }
}
