using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionGroup
{
    public SelectionGroup()
    {
        FormationHandlers[Formation.None] = new FormationHandlerNone();
        FormationHandlers[Formation.Column] = new FormationHandlerColumn();
        FormationHandlers[Formation.Line] = new FormationHandlerLine();
        FormationHandlers[Formation.Square] = new FormationHandlerSquare();
        FormationHandlers[Formation.Wedge] = new FormationHandlerWedge();
    }

    public void Add(MyGameObject myGameObject)
    {
        myGameObject.Select(true);

        Items.Add(myGameObject);
    }

    public void Add(HashSet<MyGameObject> myGameObjects)
    {
        foreach (MyGameObject myGameObject in myGameObjects)
        {
            Add(myGameObject);
        }
    }

    public void Clear()
    {
        foreach (MyGameObject selected in Items)
        {
            selected.Select(false);
        }

        Items.Clear();
    }

    public bool Contains(MyGameObject myGameObject)
    {
        return Items.Contains(myGameObject);
    }
    
    public MyGameObject First()
    {
        return Items.First();
    }

    public void Select(bool status)
    {
        foreach (MyGameObject myGameObject in Items)
        {
            myGameObject.Select(status);
        }
    }

    public void Remove(MyGameObject myGameObject)
    {
        myGameObject.Select(false);

        Items.Remove(myGameObject);
    }

    public void RemoveEmpty()
    {
        Items.RemoveWhere(x => x == null);
    }

    public void Attack(Vector3 position, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Attack(position);
        }
    }

    public void Attack(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Attack(myGameObject);
        }
    }

    public void Assemble(string prefab, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Assemble(prefab);
        }
    }

    public void Construct(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Construct(myGameObject);
        }

    }

    public void Destroy(bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Destroy();
        }
    }

    public void Disable(bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Disable();
        }
    }

    public void Enable(bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Enable();
        }
    }

    public void Explore(bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Explore();
        }
    }

    public void Follow(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Follow(myGameObject);
        }
    }

    public void Gather(bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Gather();
        }
    }

    public void Guard(Vector3 position, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Guard(position);
        }
    }

    public void Guard(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Guard(myGameObject);
        }
    }

    public void Move(Vector3 position, Formation formation = Formation.None, bool append = false)
    {
        if (FormationHandlers.TryGetValue(formation, out FormationHandler handler))
        {
            handler.Execute(this, position, append);
        }
    }

    public void Patrol(Vector3 position, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Patrol(position);
        }
    }

    public void Produce(string recipe, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Produce(recipe);
        }
    }

    public void Rally(Vector3 position, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Rally(position);
        }
    }

    public void Research(string technology, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Research(technology);
        }
    }

    public void Stop(bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Stop();
        }
    }

    public void UseSkill(string skill, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.UseSkill(skill);
        }
    }

    public void Wait(bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Wait();
        }
    }

    public int Count { get => Items.Count; }

    public HashSet<MyGameObject> Items { get; set; } = new HashSet<MyGameObject>();

    private Dictionary<Formation, FormationHandler> FormationHandlers { get; } = new Dictionary<Formation, FormationHandler>();
}
