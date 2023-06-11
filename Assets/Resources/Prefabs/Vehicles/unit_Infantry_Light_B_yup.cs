public class unit_Infantry_Light_B_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Attack);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);

        Speed = 2;
    }
}
