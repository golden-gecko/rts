using System.Collections.Generic;

public class OrderHandlerSkill : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.Prefab.Length > 0)
        {
            if (myGameObject.Skills.ContainsKey(order.Prefab))
            {
                myGameObject.Skills[order.Prefab].Execute(myGameObject);

                myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
            }
            else
            {
                myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            }
        }
        else
        {
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
        }

        myGameObject.Orders.Pop();
    }
}
