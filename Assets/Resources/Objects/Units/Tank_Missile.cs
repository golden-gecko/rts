public class Tank_Missile : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Engine = new Engine("Diesel", 6.0f);

        Gun = new Cannon("Cannon", 20.0f, 8.0f, 5.0f);
        Gun.MissilePrefab = "Objects/Missiles/Rocket";
    }
}
