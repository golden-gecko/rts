public class OrderHandlerIdleProducer : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Produce();
    }
}
