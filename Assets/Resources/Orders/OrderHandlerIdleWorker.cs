using UnityEngine;

public class OrderHandlerIdleWorker : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Game game = GameObject.Find("Game").GetComponent<Game>();

        Order order;

        order = game.CreateOrderUnload();

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = game.CreateOrderTransport();

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }

        order = game.CreateOrderConstruction();

        if (order != null)
        {
            myGameObject.Orders.Add(order);

            return;
        }
    }
}
