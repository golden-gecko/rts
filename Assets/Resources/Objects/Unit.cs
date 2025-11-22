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

        Armour = new Armour(this, "Armour", 12.0f);

        Health = 50.0f;
        MaxHealth = 50.0f;
    }
}
