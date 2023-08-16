using UnityEngine;

public class OrderHandlerWait : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return order.Timer.Max > 0.0f;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        myGameObject.Stats.Add(Stats.TimeWaiting, order.Timer.Max);
        myGameObject.Orders.Pop();
    }
}
