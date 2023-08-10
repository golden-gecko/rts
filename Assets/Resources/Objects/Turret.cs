public class Turret : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackTurret();
        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }
}
