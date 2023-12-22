using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Conveyor : Part
{
    protected override void Awake()
    {
        base.Awake();

        Item = Parent.Body.Find("Item");

        InitializeDirection();
    }

    protected override void Update()
    {
        base.Update();

        if (Parent.Storage == null)
        {
            return;
        }

        if (Parent.Storage.Resources.Empty)
        {
            ConveyorState = ConveyorState.Receiving;
        }
        else if (MoveTimer.Finished == false)
        {
            ConveyorState = ConveyorState.Moving;
        }
        else
        {
            ConveyorState = ConveyorState.Sending;
        }

        switch (ConveyorState)
        {
            case ConveyorState.Receiving:
                MyGameObject input = GetInput();

                if (input && input.Storage)
                {
                    Receive(input);
                }
                break;

            case ConveyorState.Moving:
                MoveTimer.Update(Time.deltaTime);
                break;

            case ConveyorState.Sending:
                MyGameObject output = GetOutput();

                if (output && output.Storage)
                {
                    if (Send(output))
                    {
                        MoveTimer.Reset();
                    }
                }
                break;
        }

        Item.gameObject.SetActive(Parent.Storage.Resources.Empty == false);
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
                position.x -= 1;
                break;

            case ConveyorDiretion.Up:
                position.x += 1;
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
                position.x += 1;
                break;

            case ConveyorDiretion.Up:
                position.x -= 1;
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

    private bool Receive(MyGameObject input)
    {
        foreach (Resource resource in input.Storage.Resources.Items)
        {
            if (resource.Out == false)
            {
                continue;
            }

            if (resource.Empty)
            {
                continue;
            }

            if (Parent.Storage.Resources.CanInc(resource.Name) == false)
            {
                continue;
            }

            resource.Dec();
            Parent.Storage.Resources.Inc(resource.Name);

            return true;
        }

        return false;
    }

    private bool Send(MyGameObject output)
    {
        foreach (Resource resource in Parent.Storage.Resources.Items)
        {
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

            return true;
        }

        return false;
    }

    [SerializeField]
    private Timer MoveTimer = new Timer(1.0f);

    [SerializeField]
    private ConveyorDiretion ConveyorDiretion = ConveyorDiretion.Right;

    [SerializeField]
    private ConveyorState ConveyorState = ConveyorState.Receiving;

    private Transform Item;
}
