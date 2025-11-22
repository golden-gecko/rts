public class Disaster : MyGameObject
{
    protected override void Awake()
    {
        base.Awake();

        Orders.AllowOrder(OrderType.Move);
    }
}
