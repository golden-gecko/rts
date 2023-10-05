public class OrderHandlerIdleProducer : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Produce();
    }
}
