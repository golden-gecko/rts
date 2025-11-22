using UnityEngine;

public class OrderHandlerResearch : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        order.Timer.Update(Time.deltaTime);

        if (order.Timer.Finished)
        {
            myGameObject.Player.TechnologyTree.Unlock(order.Technology);

            myGameObject.Orders.Pop();
        }
    }
}
