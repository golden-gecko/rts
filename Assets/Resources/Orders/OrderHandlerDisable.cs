using UnityEngine;

public class OrderHandlerDisable : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (myGameObject.Enabled == false)
        {
            Fail(myGameObject);

            return;
        }

        if (order.Timer == null)
        {
            order.Timer = new Timer(myGameObject.EnableTime);
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        myGameObject.Enabled = false;
        myGameObject.Orders.Pop();
    }
}
