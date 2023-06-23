public class struct_Turret_Missile_A_yup : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Attack);
    }
}
