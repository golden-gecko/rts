public class OrderHandler
{
    public virtual void OnExecute(MyGameObject myGameObject)
    {
    }

    protected virtual bool IsValid(Order order)
    {
        return true;
    }

    protected void Fail(MyGameObject myGameObject)
    {
        myGameObject.Stats.Inc(Stats.OrdersFailed);
        myGameObject.Orders.Pop();
    }
}
