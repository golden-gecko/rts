public class Turret : Structure
{
    protected override void Awake()
    {
        base.Awake();

        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackTurret();
        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }
}
