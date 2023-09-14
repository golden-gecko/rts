public class LaserMissile : Missile
{
    protected override void Awake()
    {
        base.Awake();

        MyGameObject parent = GetComponent<MyGameObject>();

        parent.Orders.AllowOrder(OrderType.Attack);

        parent.OrderHandlers[OrderType.Attack] = new OrderHandlerAttackLaser();
    }
}
