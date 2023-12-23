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
            MyGameObject input = GetInput();

            if (input && input.Storage)
            {
                Receive(input);
            }
        }

        if (Parent.Storage.Resources.Empty == false)
        {
            MoveTimer.Update(Time.deltaTime);
        }

        if (Parent.Storage.Resources.Empty == false && MoveTimer.Finished)
        {
            MyGameObject output = GetOutput();

            if (output && output.Storage)
            {
                if (Send(output))
                {
                    MoveTimer.Reset();
                }
            }
        }

        /*
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
        */

        UpdateItemPosition();
    }

    public void Put(string name, ConveyorDiretion conveyorDiretion)
    {
        ConveyorDiretionInput = GetOppositeDirection(conveyorDiretion);

        Parent.Storage.Resources.Inc(name);
    }

    private void InitializeDirection()
    {
        if (Parent.Direction.x > 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Down;
            ConveyorDiretionInput = ConveyorDiretion.Up;
        }
        else if (Parent.Direction.x < 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Up;
            ConveyorDiretionInput = ConveyorDiretion.Down;
        }
        else if (Parent.Direction.z > 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Right;
            ConveyorDiretionInput = ConveyorDiretion.Left;
        }
        else if (Parent.Direction.z < 0.0)
        {
            ConveyorDiretion = ConveyorDiretion.Left;
            ConveyorDiretionInput = ConveyorDiretion.Right;
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
                Debug.Log(string.Format("Invalid conveyor direction: {0}.", ConveyorDiretion));
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

            if (output.TryGetComponent(out Conveyor conveyor))
            {
                conveyor.Put(resource.Name, ConveyorDiretion);
            }
            /*
            else if (output.TryGetComponent(out ConveyorSplitter conveyorSplitter))
            {
                conveyorSplitter.Put(resource.Name);
            }
            */
            else
            {
                output.Storage.Resources.Inc(resource.Name);
            }

            return true;
        }

        return false;
    }

    private Vector3Int GetDirectionToOuputPosition()
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
                Debug.Log(string.Format("Invalid conveyor direction: {0}.", ConveyorDiretion));
                break;
        }

        return position;
    }

    private ConveyorDiretion GetOppositeDirection(ConveyorDiretion diretion)
    {
        switch (diretion)
        {
            case ConveyorDiretion.Down:
                return ConveyorDiretion.Up;

            case ConveyorDiretion.Up:
                return ConveyorDiretion.Down;

            case ConveyorDiretion.Right:
                return ConveyorDiretion.Left;

            case ConveyorDiretion.Left:
                return ConveyorDiretion.Right;

            default:
                Debug.Log(string.Format("Invalid conveyor direction: {0}.", ConveyorDiretion));
                return ConveyorDiretion.Left; // TODO: Correct?
        }
    }

    private void UpdateItemPosition()
    {
        // TODO: Hack for hiding item.
        if (Parent.Storage.Resources.Empty)
        {
            if (Item.gameObject.activeInHierarchy)
            {
                ItemHideFrameTimer += 1;

                if (ItemHideFrameTimer > 2)
                {
                    ItemHideFrameTimer = 0;

                    Item.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            ItemHideFrameTimer = 0;

            Item.gameObject.SetActive(true);
        }       
        // TODO: Hack end.

        Vector3 position = Item.transform.localPosition;

        position.x = 0.0f;
        position.z = 0.0f;

        float size = Parent.Size.x;
        float sizeHalf = Parent.Size.x / 2.0f;

        if (MoveTimer.Percent < 0.5f)
        {
            switch (ConveyorDiretionInput)
            {
                case ConveyorDiretion.Up:
                    switch (ConveyorDiretion)
                    {
                        case ConveyorDiretion.Up:
                            position.z = size - size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Down:
                            position.z = size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Left:
                            position.x = size - size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Right:
                            position.x = size * MoveTimer.Percent - sizeHalf;
                            break;
                    }
                    break;

                case ConveyorDiretion.Down:
                    switch (ConveyorDiretion)
                    {
                        case ConveyorDiretion.Up:
                            position.z = size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Down:
                            position.z = size - size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Left:
                            position.x = size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Right:
                            position.x = size - size * MoveTimer.Percent - sizeHalf;
                            break;
                    }
                    break;

                case ConveyorDiretion.Left:
                    switch (ConveyorDiretion)
                    {
                        case ConveyorDiretion.Up:
                            position.x = size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Down:
                            position.x = size - size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Left:
                            position.z = size - size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Right:
                            position.z = size * MoveTimer.Percent - sizeHalf;
                            break;
                    }
                    break;

                case ConveyorDiretion.Right:
                    switch (ConveyorDiretion)
                    {
                        case ConveyorDiretion.Up:
                            position.x = size - size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Down:
                            position.x = size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Left:
                            position.z = size * MoveTimer.Percent - sizeHalf;
                            break;

                        case ConveyorDiretion.Right:
                            position.z = size - size * MoveTimer.Percent - sizeHalf;
                            break;
                    }
                    break;
            }
        }
        else
        {
            position.z = size * MoveTimer.Percent - sizeHalf;
        }

        Item.transform.localPosition = position;
    }

    [SerializeField]
    private Timer MoveTimer = new Timer(1.0f);

    [SerializeField]
    private ConveyorDiretion ConveyorDiretion = ConveyorDiretion.Right;

    [SerializeField]
    private ConveyorDiretion ConveyorDiretionInput = ConveyorDiretion.Left;

    // [SerializeField]
    // private ConveyorState ConveyorState = ConveyorState.Receiving;

    private Transform Item;

    // TODO: Hack for hiding item.
    private int ItemHideFrameTimer = 0;
    // TODO: Hack end.
}
