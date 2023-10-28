using UnityEngine;

public class OrderHandlerTeleport : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (IsValid(myGameObject, order) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (order.State == null && Utils.IsCloseTo(myGameObject.Position, order.SourceGameObject.Position) == false)
        {
            myGameObject.Move(order.SourceGameObject.Position, 0);

            order.State = "GoToEntrance";

            return;
        }

        if (order.State == "GoToEntrance")
        {
            if (order.Timer == null)
            {
                order.Timer = new Timer(order.SourceGameObject.GetComponent<Teleporter>().UsageTime);
            }

            order.SourceGameObject.GetComponent<Teleporter>().Open();
            order.TargetGameObject.GetComponent<Teleporter>().Open();

            order.State = "Open";
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        if (order.State == "Open")
        {
            myGameObject.Position = order.TargetGameObject.Position;

            order.State = "GoToExit";
        }

        if (order.TargetGameObject != null && Utils.IsCloseTo(myGameObject.Position, order.TargetGameObject.Exit) == false)
        {
            myGameObject.Move(order.TargetGameObject.Exit, 0);

            return;
        }

        order.SourceGameObject.GetComponent<Teleporter>().Close();
        order.TargetGameObject.GetComponent<Teleporter>().Close();

        Success(myGameObject);
    }

    protected override bool IsValid(MyGameObject myGameObject, Order order)
    {
        return order.SourceGameObject != null && order.SourceGameObject != myGameObject && order.TargetGameObject != null && order.TargetGameObject != myGameObject;
    }
}
