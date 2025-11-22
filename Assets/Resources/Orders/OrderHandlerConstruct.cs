using UnityEngine;

public class OrderHandlerConstruct : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            myGameObject.Orders.Pop();

            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
        }
        else if (myGameObject.IsCloseTo(order.TargetGameObject.Entrance) == false)
        {
            myGameObject.Move(order.TargetGameObject.Entrance, 0);
        }
        else if (order.TargetGameObject.Constructed)
        {
            order.Timer.Update(Time.deltaTime);

            if (order.Timer.Finished)
            {
                order.TargetGameObject.State = MyGameObjectState.Operational;
                order.Timer.Reset();

                myGameObject.Orders.Pop();

                myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
                myGameObject.Stats.Add(Stats.TimeConstructing, order.Timer.Max);
            }
        }
        else
        {
            myGameObject.Wait(0);
        }
    }
}
