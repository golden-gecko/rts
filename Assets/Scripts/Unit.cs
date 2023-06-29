public class Unit : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Follow);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Health = 50.0f;
        MaxHealth = 50.0f;
        MissileRangeMin = 1.0f;
    }
}
