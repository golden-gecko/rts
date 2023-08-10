public class Tank_Missile : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Engine = new Engine(this, "Diesel", 1.0f, 6.0f);

        Gun = new Cannon(this, "Cannon", 1.0f, 20.0f, 8.0f, 5.0f);
        Gun.MissilePrefab = "Objects/Missiles/Rocket";
    }
}
