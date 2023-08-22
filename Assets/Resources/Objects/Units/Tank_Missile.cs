public class Tank_Missile : Vehicle
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
    }
}
