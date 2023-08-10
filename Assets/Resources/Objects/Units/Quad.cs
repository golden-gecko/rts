public class Quad : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Gun = new Cannon("Cannon", 2.0f, 6.0f, 3.0f);
        Gun.MissilePrefab = "Objects/Missiles/Rocket";
    }
}
