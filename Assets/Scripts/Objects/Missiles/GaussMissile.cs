using UnityEngine;

[DisallowMultipleComponent]
public class GaussMissile : Missile
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.AttackObject);
        Orders.AllowOrder(OrderType.AttackPosition);

        OrderHandlers[OrderType.AttackObject] = new OrderHandlerAttackObjectGauss();
        OrderHandlers[OrderType.AttackPosition] = new OrderHandlerAttackPositionGauss();
    }
}
