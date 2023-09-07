public class OrderHandlerDestroy : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        myGameObject.OnDestroy_();
        myGameObject.Orders.Pop();
    }
}
