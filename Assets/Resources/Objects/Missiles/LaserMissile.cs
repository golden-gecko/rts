public class LaserMissile : Missile
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackLaser();
    }
}
