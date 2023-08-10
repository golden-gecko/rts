public class Infantry_Light : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Engine = new Engine("Diesel", 2.0f);

        Gun = new Cannon("Cannon", 1.0f, 3.0f, 1.0f);
        Gun.MissilePrefab = "Objects/Missiles/Bullet";
    }
}
