public class Turret : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackTurret();
        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();

        Damage = 10.0f;
        MissileRange = 10.0f;
        MissilePrefab = "Objects/Missiles/Rocket";
        ReloadTimer = new Timer(2.0f);
    }
}
