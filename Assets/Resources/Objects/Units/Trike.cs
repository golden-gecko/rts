public class Trike : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 12.0f;
        MissilePrefab = "Objects/Missiles/Bullet";
        ReloadTimer = new Timer(2.0f);
    }
}
