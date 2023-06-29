using UnityEngine;

public class OrderHandlerPatrol : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.Move(order.TargetPosition);
        myGameObject.Move(myGameObject.Position);

        myGameObject.Orders.MoveToEnd();
    }
}
