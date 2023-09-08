public class Thruster : Engine // TODO: Merge into one engine.
{
    protected override void Awake()
    {
        base.Awake();

        GetComponent<MyGameObject>().OrderHandlers[OrderType.Move] = new OrderHandlerMoveMissile();
    }
}
