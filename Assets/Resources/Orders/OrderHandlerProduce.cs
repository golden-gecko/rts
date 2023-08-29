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

        if (order.Recipe.Length <= 0 && myGameObject.GetComponent<Producer>().Recipes.Items.Count > 0)
        {
            order.Recipe = myGameObject.GetComponent<Producer>().Recipes.Items.First().Key; // TODO: Find recipe that can be produced.
        }
        else if (myGameObject.GetComponent<Producer>().Recipes.Items.ContainsKey(order.Recipe) == false)
        {
            myGameObject.Stats.Inc(Stats.OrdersFailed);
            myGameObject.Orders.Pop();

            return;
        }

        Produce(myGameObject, order, myGameObject.GetComponent<Producer>().Recipes.Items[order.Recipe]);
    }

    private bool Produce(MyGameObject myGameObject, Order order, Recipe recipe)
    {
        if (order.Timer == null)
        {
            order.Timer = new Timer(recipe.Sum / myGameObject.GetComponent<Producer>().ResourceUsage);
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

        myGameObject.Stats.Inc(Stats.OrdersCompleted);
        myGameObject.Stats.Add(Stats.TimeProducing, order.Timer.Max);
        myGameObject.Orders.Pop();

        return true;
    }

    private bool HaveResources(MyGameObject myGameObject, Recipe recipe)
    {
        foreach (Resource i in recipe.ToConsume.Items.Values)
        {
            if (myGameObject.GetComponent<Storage>().Resources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        foreach (Resource i in recipe.ToProduce.Items.Values)
        {
            if (myGameObject.GetComponent<Storage>().Resources.CanAdd(i.Name, i.Max) == false)
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
            myGameObject.GetComponent<Storage>().Resources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }

        foreach (Resource i in recipe.ToProduce.Items.Values)
        {
            myGameObject.GetComponent<Storage>().Resources.Add(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesProduced, i.Max);
        }
    }
}
