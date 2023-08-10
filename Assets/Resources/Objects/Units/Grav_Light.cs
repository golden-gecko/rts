public class Grav_Light : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        MissilePrefab = "Objects/Missiles/Rocket";
        Speed = 8.0f;
        Gun = new Gauss("Gauss", 10.0f, 20.0f, 2.0f);
    }
}
