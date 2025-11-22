using UnityEngine;

public class OrderHandlerAssemble : OrderHandler
{
    public override void OnExecuteHandler(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            order.TargetGameObject = Utils.CreateGameObject(order.Prefab, myGameObject.Exit, Quaternion.identity, myGameObject.Player, MyGameObjectState.UnderAssembly);
        }

        ResourceContainer resourceContainer = order.TargetGameObject.ConstructionResources;

        if (order.Timer == null)
        {
            order.Timer = new Timer(Mathf.Ceil((float)resourceContainer.MaxSum / myGameObject.GetComponent<Assembler>().ResourceUsage));
        }

        if (HaveResources(myGameObject, resourceContainer) == false)
        {
            myGameObject.Wait(0);

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        MoveResources(myGameObject, resourceContainer);

        order.TargetGameObject.SetState(MyGameObjectState.Operational);
        order.TargetGameObject.Move(myGameObject.GetComponent<Assembler>().RallyPoint, 0);

        myGameObject.Stats.Inc(Stats.ObjectsAssembled);
        myGameObject.Stats.Add(Stats.TimeAssembling, order.Timer.Max);

        Success(myGameObject);
    }

    private bool HaveResources(MyGameObject myGameObject, ResourceContainer resourceContainer)
    {
        foreach (Resource i in resourceContainer.Items)
        {
            if (myGameObject.GetComponent<Storage>().Resources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, ResourceContainer resourceContainer)
    {
        foreach (Resource i in resourceContainer.Items)
        {
            myGameObject.GetComponent<Storage>().Resources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }
    }
}
