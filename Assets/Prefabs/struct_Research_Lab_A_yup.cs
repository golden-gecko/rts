public class struct_Research_Lab_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Research);
    }
}
