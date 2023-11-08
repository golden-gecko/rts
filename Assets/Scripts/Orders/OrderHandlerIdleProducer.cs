public class OrderHandlerIdleProducer : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        myGameObject.Produce();
    }
}
