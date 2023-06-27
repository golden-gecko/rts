public class OrderHandlerIdleProduce : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.Produce();
    }
}
