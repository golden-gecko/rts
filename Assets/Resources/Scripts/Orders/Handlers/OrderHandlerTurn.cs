using UnityEngine;

public class OrderHandlerTurn : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        myGameObject.transform.LookAt(new Vector3(order.TargetPosition.x, myGameObject.Position.y, order.TargetPosition.z));

        Success(myGameObject);
    }
}
