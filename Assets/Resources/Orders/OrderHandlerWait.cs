using UnityEngine;

public class OrderHandlerWait : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished)
        {
            order.Timer.Reset();

            myGameObject.Stats.Add(Stats.TimeWaiting, order.Timer.Max);
            myGameObject.Orders.Pop();
        }
    }
}
