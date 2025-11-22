using System.Linq;
using UnityEngine;

public class OrderHandlerAssemble : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.TargetGameObject == null)
        {
            order.TargetGameObject = Utils.CreateGameObject(order.Prefab, myGameObject.Exit, myGameObject.Player, MyGameObjectState.UnderAssembly);
            order.TargetGameObject.GetComponentInChildren<Indicators>().OnUnderConstruction();
            order.TargetGameObject.RaiseConstructionResourceFlags();
        }

        Recipe recipe = order.TargetGameObject.ConstructionRecipies.Items.First().Value;

        if (order.Timer == null)
        {
            order.Timer = new Timer(recipe.MaxSum / myGameObject.GetComponent<Assembler>().ResourceUsage);
        }

        if (HaveResources(myGameObject, recipe) == false)
        {
            myGameObject.Wait(0);

            return;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return;
        }

        MoveResources(myGameObject, recipe);

        order.TargetGameObject.State = MyGameObjectState.Operational;
        order.TargetGameObject.Move(myGameObject.GetComponent<Assembler>().RallyPoint, 0);
        order.TargetGameObject.GetComponentInChildren<Indicators>().OnConstructionCompleted();
        order.TargetGameObject.RemoveConstructionResourceFlags();

        myGameObject.Stats.Inc(Stats.OrdersCompleted);
        myGameObject.Stats.Inc(Stats.ObjectsAssembled);
        myGameObject.Stats.Add(Stats.TimeAssembling, order.Timer.Max);
        myGameObject.Orders.Pop();
    }

    private bool HaveResources(MyGameObject myGameObject, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items)
        {
            if (myGameObject.GetComponent<Storage>().Resources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items)
        {
            myGameObject.GetComponent<Storage>().Resources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }
    }
}
