public class Quad : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 12.0f;
        MissilePrefab = "Objects/Missiles/Bullet";
        Gun = new Cannon("Cannon", 2.0f, 6.0f, 3.0f);
    }
}
