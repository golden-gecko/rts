public class Trike : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 12.0f;
        MissilePrefab = "Objects/Missiles/Bullet";
        Gun = new Laser("Laser", 2.0f, 4.0f, 2.0f);
    }
}
