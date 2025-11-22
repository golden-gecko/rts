using System.Collections.Generic;
using System.Linq;

public class OrderContainer
{
    public void Add(Order item)
    {
        if (OrderWhitelist.Contains(item.Type))
        {
            Items.Add(item);
        }
    }

    public void AllowOrder(OrderType orderType)
    {
        OrderWhitelist.Add(orderType);
    }

    public void AllowPrefab(string prefab)
    {
        PrefabWhitelist.Add(prefab);
    }

    public void AllowRecipe(Recipe recipe)
    {
        RecipeWhitelist.Add(recipe);
    }

    public void AllowTechnology(string technology)
    {
        TechnologyWhitelist.Add(technology);
    }

    public void Clear()
    {
        foreach (Order item in Items)
        {
            item.Cancel();
        }

        Items.Clear();
    }

    public Order First()
    {
        return Items[0];
    }

    public string GetInfo()
    {
        return string.Join("\n", Items.Select(x => x.GetInfo()));
    }

    public void Insert(int index, Order item)
    {
        Items.Insert(index, item);
    }

    public bool IsAllowed(OrderType item)
    {
        return OrderWhitelist.Contains(item);
    }

    public void MoveToEnd()
    {
        if (Items.Count > 1)
        {
            Add(Items[0]);
            Pop();
        }
    }

    public void Pop()
    {
        Items.RemoveAt(0);
    }

    public int Count { get => Items.Count; }

    public List<Order> Items { get; } = new List<Order>();

    public HashSet<OrderType> OrderWhitelist { get; } = new HashSet<OrderType>();

    public HashSet<string> PrefabWhitelist { get; } = new HashSet<string>();

    public RecipeContainer RecipeWhitelist { get; } = new RecipeContainer();

    public HashSet<string> TechnologyWhitelist { get; } = new HashSet<string>();
}
