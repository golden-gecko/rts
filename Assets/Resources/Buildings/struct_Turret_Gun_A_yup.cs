public class struct_Turret_Gun_A_yup : Structure
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
    }
}
