public class Unit : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();
       
        OrderHandlers[OrderType.Attack] = new OrderHandlerAttackUnit();
        OrderHandlers[OrderType.Idle] = new OrderHandlerIdleAttacker();
    }
}
