public class Truck : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Move);
    }

    protected override void Update()
    {
        base.Update();
    }
}
