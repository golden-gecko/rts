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

        MyGameObject input = GetInput();
        MyGameObject output = GetOutput();


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

        // TODO: Check index range.
        if (Map.Instance.Cells[position.x, position.z].Occupied.TryGetValue(Parent.Player, out ValueGameObjects value) == false)
        {
            return null;
        }

        if (value.GameObjects.Count <= 0)
        {
            return null;
        }

        return value.GameObjects.First();
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

        // TODO: Check index range.
        if (Map.Instance.Cells[position.x, position.z].Occupied.TryGetValue(Parent.Player, out ValueGameObjects value) == false)
        {
            return null;
        }

        if (value.GameObjects.Count <= 0)
        {
            return null;
        }

        return value.GameObjects.First();
    }

    public ConveyorDiretion ConveyorDiretion { get; private set; } = ConveyorDiretion.Right;
}
