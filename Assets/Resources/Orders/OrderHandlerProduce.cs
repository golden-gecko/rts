using System.Linq;
using UnityEngine;

public class OrderHandlerProduce : IOrderHandler
{
    public bool IsValid(Order order)
    {
        return true;
    }

    public void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        if (order.Recipe.Length <= 0 && myGameObject.Recipes.Items.Count > 0)
        {
            order.Recipe = myGameObject.Recipes.Items.First().Key; // TODO: Find recipe that can be produced.
        }

        if (myGameObject.Recipes.Items.ContainsKey(order.Recipe) == false)
        {
            myGameObject.Stats.Add(Stats.OrdersFailed, 1);
            myGameObject.Orders.Pop();
        }

        Produce(myGameObject, order, myGameObject.Recipes.Items[order.Recipe]);
    }

    private bool Produce(MyGameObject myGameObject, Order order, Recipe recipe)
    {
        if (order.Timer == null)
        {
            order.Timer = new Timer(recipe.Total / order.ResourceUsage);
        }

        if (HaveResources(myGameObject, recipe) == false)
        {
            myGameObject.Wait(0);

            return false;
        }

        if (order.Timer.Update(Time.deltaTime) == false)
        {
            return false;
        }

        MoveResources(myGameObject, recipe);

        myGameObject.Stats.Add(Stats.OrdersExecuted, 1);
        myGameObject.Stats.Add(Stats.TimeProducing, order.Timer.Max);
        myGameObject.Orders.Pop();

        return true;
    }

    private bool HaveResources(MyGameObject myGameObject, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items.Values)
        {
            if (myGameObject.Resources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        foreach (Resource i in recipe.ToProduce.Items.Values)
        {
            if (myGameObject.Resources.CanAdd(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        return true;
    }

    private void MoveResources(MyGameObject myGameObject, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items.Values)
        {
            myGameObject.Resources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }

        foreach (Resource i in recipe.ToProduce.Items.Values)
        {
            myGameObject.Resources.Add(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesProduced, i.Max);
        }
    }
}
