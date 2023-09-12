using UnityEngine;

public class OrderHandlerProduce : OrderHandler
{
    public override void OnExecute(MyGameObject myGameObject)
    {
        Order order = myGameObject.Orders.First();

        RecipeManager recipeManager = Game.Instance.GetComponent<RecipeManager>();

        if (order.Recipe.Length <= 0 && myGameObject.Orders.RecipeWhitelist.Items.Count > 0)
        {
            foreach (Recipe recipe in myGameObject.Orders.RecipeWhitelist.Items.Values)
            {
                if (HaveResources(myGameObject, recipe))
                {
                    order.Recipe = recipe.Name;

                    break;
                }
            }
        }
        else if (myGameObject.Orders.RecipeWhitelist.Items.ContainsKey(order.Recipe) == false)
        {
            Fail(myGameObject);

            return;
        }

        if (order.Recipe.Length <= 0)
        {
            myGameObject.Wait(0);

            return;
        }

        Produce(myGameObject, order, myGameObject.Orders.RecipeWhitelist.Items[order.Recipe]);
    }

    private bool Produce(MyGameObject myGameObject, Order order, Recipe recipe)
    {
        if (order.Timer == null)
        {
            order.Timer = new Timer(recipe.MaxSum / myGameObject.GetComponent<Producer>().ResourceUsage);
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
        foreach (Resource i in recipe.ToConsume.Items)
        {
            if (myGameObject.GetComponent<Storage>().Resources.CanRemove(i.Name, i.Max) == false)
            {
                return false;
            }
        }

        foreach (Resource i in recipe.ToProduce.Items)
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
        foreach (Resource i in recipe.ToConsume.Items)
        {
            myGameObject.GetComponent<Storage>().Resources.Remove(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesUsed, i.Max);
        }

        foreach (Resource i in recipe.ToProduce.Items)
        {
            myGameObject.GetComponent<Storage>().Resources.Add(i.Name, i.Max);
            myGameObject.Stats.Add(Stats.ResourcesProduced, i.Max);
        }
    }
}
