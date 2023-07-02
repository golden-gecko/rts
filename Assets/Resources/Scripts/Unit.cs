public class Unit : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Follow);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);
        
        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackUnit();

        Damage = 10.0f;
        Health = 50.0f;
        MaxHealth = 50.0f;
        MissileRangeMax = 10.0f;
        MissileRangeMin = 1.0f;
    }
}
