public class Turret : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Damage = 10.0f;
        MissileRangeMin = 2.0f;
        MissileRangeMin = 20.0f;
    }
}
