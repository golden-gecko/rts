public class struct_Turret_Missile_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Attack);
    }
}
