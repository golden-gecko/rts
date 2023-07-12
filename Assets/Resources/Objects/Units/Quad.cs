public class Quad : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 12.0f;
        MissilePrefab = "Objects/Missiles/Bullet";
        ReloadTimer = new Timer(3.0f);
    }
}
