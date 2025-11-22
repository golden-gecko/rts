using UnityEngine;

public class OrderHandlerConstruct : IOrderHandler
{
    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (myGameObject.IsCloseTo(order.TargetPosition + new Vector3(0, 0, 1)) == false)
        {
            myGameObject.Move(order.TargetPosition + new Vector3(0, 0, 1), 0); // TODO: Add offset based on object size.
        }
        else if (order.TargetGameObject == null)
        {
            MyGameObject resource = Resources.Load<MyGameObject>(order.Prefab);

            order.TargetGameObject = Object.Instantiate<MyGameObject>(resource, order.TargetPosition, Quaternion.identity);
            order.TargetGameObject.State = MyGameObjectState.UnderConstruction;
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
