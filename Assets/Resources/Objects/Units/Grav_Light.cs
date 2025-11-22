public class Grav_Light : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Engine = new Engine(this, "Diesel", 8.0f);

        Gun = new Gauss(this, "Gauss", 10.0f, 10.0f, 2.0f);
        Gun.MissilePrefab = "Objects/Missiles/Gauss";
    }
}
