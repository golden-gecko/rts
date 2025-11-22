public class struct_Factory_Heavy_A_yup : MyGameObject
{
    protected override void Start()
    {
        base.Start();

        Orders.Allow(OrderType.Construct);
    }
}
