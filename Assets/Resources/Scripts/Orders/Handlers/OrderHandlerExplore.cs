using UnityEngine;

public class OrderHandlerExplore : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (Map.Instance.GetNearestUnexplored(order, myGameObject.Player, out Vector3 position))
        {
            myGameObject.Move(position, 0);
        }
        else
        {
            Fail(myGameObject);
        }
    }
}
