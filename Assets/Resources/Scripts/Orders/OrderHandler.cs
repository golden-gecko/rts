public class OrderHandler
{
    public virtual void OnExecuteHandler(MyGameObject myGameObject)
    {
    }

    protected virtual bool IsValid(MyGameObject myGameObject, Order order)
    {
        return true;
    }

    protected void Fail(MyGameObject myGameObject)
    {
        myGameObject.Stats.Inc(Stats.OrdersFailed);
        myGameObject.Orders.Pop();
        myGameObject.Wait(0);
    }

    protected void Success(MyGameObject myGameObject)
    {
        myGameObject.Stats.Inc(Stats.OrdersCompleted);
        myGameObject.Orders.Pop();
    }
}
