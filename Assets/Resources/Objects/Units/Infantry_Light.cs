public class Infantry_Light : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        MissilePrefab = "Objects/Missiles/Bullet";
        Speed = 2.0f;
        Gun = new Cannon("Cannon", 1.0f, 3.0f, 1.0f);
    }
}
