public class Unit : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Explore);
        Orders.AllowOrder(OrderType.Follow);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);
        
        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackUnit();
        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }
}
