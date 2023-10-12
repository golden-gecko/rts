using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SelectionGroup
{
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

    private void MoveNoFormation(Vector3 position, bool append = false)
    {
        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            selected.Move(position);
        }
    }

    private void MoveColumnFormation(Vector3 position, bool append = false)
    {
        if (Items.Count <= 0)
        {
            return;
        }

        Vector3 start = Vector3.zero;

        foreach (MyGameObject selected in Items)
        {
            start += selected.Position;
        }

        start /= Items.Count;

        Vector3 direction = (new Vector3(position.x, 0.0f, position.z) - new Vector3(start.x, 0.0f, start.z)).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, direction).normalized;

        int unitsCount = Items.Where(x => x.TryGetComponent(out Engine _)).Count();
        int size = Mathf.CeilToInt(Mathf.Sqrt(unitsCount));

        float spacing = 5.0f;
        float row = 0.0f;

        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(-direction.x * row, 0.0f, -direction.z * row);

            selected.Move(position + positionInFormation);

            row += spacing;
        }
    }

    private void MoveLineFormation(Vector3 position, bool append = false)
    {
        if (Items.Count <= 0)
        {
            return;
        }

        Vector3 start = Vector3.zero;

        foreach (MyGameObject selected in Items)
        {
            start += selected.Position;
        }

        start /= Items.Count;

        Vector3 direction = (new Vector3(position.x, 0.0f, position.z) - new Vector3(start.x, 0.0f, start.z)).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, direction).normalized;

        int unitsCount = Items.Where(x => x.TryGetComponent(out Engine _)).Count();
        int size = Mathf.CeilToInt(Mathf.Sqrt(unitsCount));

        float spacing = 5.0f;
        float column;

        if (unitsCount == 1)
        {
            column = 0.0f;
        }
        else if (unitsCount % 2 == 0)
        {
            column = -(unitsCount / 2) * spacing + spacing / 2.0f;
        }
        else
        {
            column = -((unitsCount - 1) / 2) * spacing;
        }

        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(cross.x * column, 0.0f, cross.z * column);

            selected.Move(position + positionInFormation);

            column += spacing;
        }
    }

    private void MoveSquareFormation(Vector3 position, bool append = false)
    {
        if (Items.Count <= 0)
        {
            return;
        }

        Vector3 start = Vector3.zero;

        foreach (MyGameObject selected in Items)
        {
            start += selected.Position;
        }

        start /= Items.Count;

        Vector3 direction = (new Vector3(position.x, 0.0f, position.z) - new Vector3(start.x, 0.0f, start.z)).normalized;
        Vector3 cross = Vector3.Cross(Vector3.up, direction).normalized;

        int unitsCount = Items.Where(x => x.TryGetComponent(out Engine _)).Count();
        int size = Mathf.CeilToInt(Mathf.Sqrt(unitsCount));

        float spacing = 5.0f;
        float column = 0.0f;
        float row = 0.0f;
        float offset;

        if (unitsCount == 1)
        {
            column = 0;
        }
        else if (unitsCount % 2 == 0)
        {
            column = -(size / 2) * offset + offset / 2.0f;
        }
        else
        {
            column = -((size - 1) / 2) * offset;
        }

        foreach (MyGameObject selected in Items)
        {
            if (append == false)
            {
                selected.ClearOrders();
            }

            Vector3 positionInFormation = new Vector3(row, 0.0f, column);

            selected.Move(position + positionInFormation);

            column += spacing;

            if (column >= size * spacing)
            {
                column = 0.0f;
                row += spacing;
            }
        }
    }

    public void Move(Vector3 position, bool append = false)
    {
        // MoveNoFormation(position, append);
        // MoveColumnFormation(position, append);
        // MoveLineFormation(position, append);
        MoveSquareFormation(position, append);
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
}
