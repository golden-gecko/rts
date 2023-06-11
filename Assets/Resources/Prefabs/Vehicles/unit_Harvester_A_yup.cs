public class unit_Harvester_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.AllowOrder(OrderType.Load);
        Orders.AllowOrder(OrderType.Move);
        Orders.AllowOrder(OrderType.Patrol);
        Orders.AllowOrder(OrderType.Unload);
        Orders.AllowOrder(OrderType.Transport);

        Speed = 4;
    }

    protected override void OnOrderIdle()
    {
        Produce();
    }
}
