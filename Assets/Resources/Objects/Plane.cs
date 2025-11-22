public class Plane : Unit
{
    protected override void Awake()
    {
        base.Awake();

        OrderHandlers[OrderType.Move] = new OrderHandlerMovePlane();
    }
}
