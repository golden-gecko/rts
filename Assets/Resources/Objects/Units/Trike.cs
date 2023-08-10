public class Trike : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Engine = new Engine(this, "Diesel", 12.0f);

        Gun = new Laser(this, "Laser", 2.0f, 4.0f, 2.0f);
        Gun.MissilePrefab = "Objects/Missiles/Laser";
    }
}
