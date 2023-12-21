using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Conveyor : Part
{
    protected override void Awake()
    {
        base.Awake();

        InitializeDirection();
    }

    protected override void Update()
    {
        base.Update();

        if (Parent.Storage == null)
        {
            return;
        }

        MyGameObject output = GetOutput();

        if (output.Storage)
        {
            MoveTo(output);
        }

        MyGameObject input = GetInput();

        if (input.Storage)
        {
            MoveFrom(input);
        }
    }

    private void InitializeDirection()
    {
        if (Parent.Direction.x > 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Down;
        }
        else if (Parent.Direction.x < 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Up;
        }
        else if (Parent.Direction.z > 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Right;
        }
        else if (Parent.Direction.z < 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Left;
        }
    }

    private MyGameObject GetInput()
    {
        Vector3Int position = Parent.PositionGrid;

        switch (ConveyorDiretion)
        {
            case ConveyorDiretion.Down:
                position.x += 1;
                break;

            case ConveyorDiretion.Up:
                position.x -= 1;
                break;

            case ConveyorDiretion.Right:
                position.z -= 1;
                break;

            case ConveyorDiretion.Left:
                position.z += 1;
                break;

            default:
                return null;
        }

        return GetGameObject(position);
    }

    private MyGameObject GetOutput()
    {
        Vector3Int position = Parent.PositionGrid;

        switch (ConveyorDiretion)
        {
            case ConveyorDiretion.Down:
                position.x -= 1;
                break;

            case ConveyorDiretion.Up:
                position.x += 1;
                break;

            case ConveyorDiretion.Right:
                position.z += 1;
                break;

            case ConveyorDiretion.Left:
                position.z -= 1;
                break;

            default:
                return null;
        }

        return GetGameObject(position);
    }

    private MyGameObject GetGameObject(Vector3Int position)
    {
        if (Map.Instance.Cells[position.x, position.z].Occupied.TryGetValue(Parent.Player, out ValueGameObjects value) == false) // TODO: Check index range.
        {
            return null;
        }

        if (value.GameObjects.Count <= 0)
        {
            return null;
        }

        return value.GameObjects.First();
    }

    private void MoveTo(MyGameObject output)
    {

    }

    private void MoveFrom(MyGameObject input)
    {
        foreach (Resource resource in input.Storage.Resources.Items)
        {
            if (resource.In == false)
            {
                continue;
            }

            if (resource.Empty)
            {
                continue;
            }

            if (output.Storage.Resources.CanInc(resource.Name) == false)
            {
                continue;
            }

            resource.Dec();
            output.Storage.Resources.Inc(resource.Name);

            break;
        }
    }

    public ConveyorDiretion ConveyorDiretion { get; private set; } = ConveyorDiretion.Right;
}
