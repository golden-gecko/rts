public class struct_Factory_Light_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Construct);
    }
}
