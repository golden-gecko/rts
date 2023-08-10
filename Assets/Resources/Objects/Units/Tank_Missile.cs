public class Tank_Missile : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 6.0f;
        MissilePrefab = "Objects/Missiles/Rocket";
        Gun = new Cannon("Cannon", 20.0f, 8.0f, 5.0f);
    }
}
