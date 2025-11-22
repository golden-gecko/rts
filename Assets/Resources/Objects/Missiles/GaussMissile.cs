public class GaussMissile : Missile
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackGauss();
    }
}
