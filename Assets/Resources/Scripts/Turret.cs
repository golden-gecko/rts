public class Turret : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackTurret();

        Damage = 10.0f;
        MissileRangeMax = 20.0f;
        MissileRangeMin = 2.0f;
    }
}
