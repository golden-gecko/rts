public class Tank_Missile : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Speed = 6.0f;
        MissilePrefab = "Objects/Missiles/Rocket";
        ReloadTimer = new Timer(5.0f);
    }
}
