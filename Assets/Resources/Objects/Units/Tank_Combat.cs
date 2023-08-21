public class Tank_Combat : Vehicle
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Skills["Damage"] = new Damage("Damage", 3.0f, 10.0f, 4.0f);
    }
}
