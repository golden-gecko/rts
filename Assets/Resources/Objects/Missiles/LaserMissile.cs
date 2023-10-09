public class LaserMissile : Missile
{
    protected override void Awake()
    {
        base.Awake();

        parent.Orders.AllowOrder(OrderType.Attack);

        parent.OrderHandlers[OrderType.Attack] = new OrderHandlerAttackLaser();
    }
}
