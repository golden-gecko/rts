public class Ship : Unit
{
    protected override void Awake()
    {
        base.Awake();

        OrderHandlers[OrderType.Move] = new OrderHandlerMoveShip();
    }
}
