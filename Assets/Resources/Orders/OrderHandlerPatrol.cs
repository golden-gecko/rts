using UnityEngine;

public class OrderHandlerPatrol : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            myGameObject.Move(order.TargetPosition);
            myGameObject.Move(myGameObject.Position);
        }
        else
        {
            myGameObject.Move(order.TargetGameObject);
            myGameObject.Move(myGameObject.Position);
        }

        myGameObject.Orders.MoveToEnd();
    }
}
