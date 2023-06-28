using System.Collections.Generic;
using UnityEngine;

public class OrderHandlerIdleWorker : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order;
        
        /*
        order = Game.Instance.CreateOrderUnload(myGameObject);

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }
        */

        order = Game.Instance.CreateOrderTransport();

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = Game.Instance.CreateOrderConstruction();

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }
    }
}
