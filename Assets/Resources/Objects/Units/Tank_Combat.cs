public class Tank_Combat : Unit
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);

        Skills["Damage"] = new Damage("Damage", 3.0f, 10.0f, 4.0f);
        Skills["Repair"] = new Repair("Repair", 3.0f, 4.0f, 20.0f);

        Speed = 6.0f;
        Gun = new Laser("Laser", 3.0f, 10.0f, 5.0f);
        Gun.MissilePrefab = "Objects/Missiles/Laser";
    }
}
