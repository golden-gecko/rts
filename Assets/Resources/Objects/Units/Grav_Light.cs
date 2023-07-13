public class Grav_Light : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        MissilePrefab = "Objects/Missiles/Rocket";
        ReloadTimer = new Timer(2.0f);
        Speed = 8.0f;
    }
}
