public class OrderHandlerIdleProducer : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Produce();
    }
}
