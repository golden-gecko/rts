public class unit_Tank_Combat_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Attack);
        Orders.Allow(OrderType.Move);
        Orders.Allow(OrderType.Patrol);

        Speed = 6;
    }
}
