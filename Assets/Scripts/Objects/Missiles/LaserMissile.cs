using UnityEngine;

[DisallowMultipleComponent]
public class LaserMissile : Missile
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.AttackObject);
        Orders.AllowOrder(OrderType.AttackPosition);

        OrderHandlers[OrderType.AttackObject] = new OrderHandlerAttackObjectLaser();
        OrderHandlers[OrderType.AttackPosition] = new OrderHandlerAttackPositionLaser();
    }
}
