using UnityEngine;

public class OrderHandlerWait : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.Timer == null)
        {
            order.Timer = new Timer(myGameObject.WaitTime);
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        myGameObject.Stats.Add(Stats.TimeWaiting, order.Timer.Max);
        myGameObject.Orders.Pop();
    }
}
