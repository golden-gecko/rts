using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionGroup
{
    public SelectionGroup()
    {
        FormationHandlers[Formation.None] = new FormationHandlerNone();
        FormationHandlers[Formation.Arrow] = new FormationHandlerArrow();
        FormationHandlers[Formation.Column] = new FormationHandlerColumn();
        FormationHandlers[Formation.Diamond] = new FormationHandlerDiamond();
        FormationHandlers[Formation.Line] = new FormationHandlerLine();
        FormationHandlers[Formation.Square] = new FormationHandlerSquare();
        FormationHandlers[Formation.Triangle] = new FormationHandlerTriangle();
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

    public void Replace(HashSet<MyGameObject> newItems)
    {
        Items = new HashSet<MyGameObject>(newItems);
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

    public void AttackObject(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.AttackObject(myGameObject);
        }
    }

    public void AttackPosition(Vector3 position, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.AttackPosition(position);
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

    public void Default(Vector3 position, bool append = false)
    {
        Move(position, HUD.Instance.Formation, append);
    }

    public void Default(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            if (myGameObject.Is(selected, DiplomacyState.Ally) && myGameObject.State == MyGameObjectState.UnderConstruction)
            {
                selected.Construct(myGameObject);
            }
            else if (myGameObject.Is(selected, DiplomacyState.Ally))
            {
                selected.Follow(myGameObject);
            }
            else if (myGameObject.Is(selected, DiplomacyState.Enemy))
            {
                selected.AttackObject(myGameObject);
            }
            else
            {
                Storage storage = myGameObject.GetComponentInChildren<Storage>();

                if (storage && storage.Gatherable)
                {
                    selected.GatherObject(myGameObject);
                }
            }
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

    public void GatherObject(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.GatherObject(myGameObject);
        }
    }

    public void GuardObject(MyGameObject myGameObject, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.GuardObject(myGameObject);
        }
    }

    public void GuardPosition(Vector3 position, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.GuardPosition(position);
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

    public HashSet<MyGameObject> Items { get; private set; } = new HashSet<MyGameObject>();

    private Dictionary<Formation, FormationHandler> FormationHandlers { get; } = new Dictionary<Formation, FormationHandler>();
}
