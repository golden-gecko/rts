using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class ConveyorSplitter : Part
{
    protected override void Awake()
    {
        base.Awake();

        Item[0] = Parent.Body.Find("Body_0001").transform.Find("Item");
        Item[1] = Parent.Body.Find("Body_0002").transform.Find("Item");

        InitializeDirection();
    }

    protected override void Update()
    {
        base.Update();

        Item[0].gameObject.SetActive(false);
        Item[1].gameObject.SetActive(false);

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
                MyGameObject[] inputs = GetInputs();

                for (int i = 0; i < inputs.Length; i++)
                {
                    if (inputs[i] && inputs[i].Storage)
                    {
                        if (Receive(inputs[i]))
                        {
                            Item[i].gameObject.SetActive(true);

                            break;
                        }
                    }
                }
                break;

            case ConveyorState.Moving:
                MoveTimer.Update(Time.deltaTime);
                break;

            case ConveyorState.Sending:
                MyGameObject[] outputs = GetOutputs();

                foreach (MyGameObject output in outputs)
                {
                    if (output && output.Storage)
                    {
                        if (Send(output))
                        {
                            MoveTimer.Reset();

                            break;
                        }
                    }
                }
                break;
        }
    }

    public void Put(string name, Vector3Int position)
    {
        Parent.Storage.Resources.Inc(name);
    }

    private void InitializeDirection()
    {
        if (Parent.Direction.x > 0.0)
        {
            ConveyorDirection = ConveyorDiretion.Down;
        }
        else if (Parent.Direction.x < 0.0)
        {
            ConveyorDirection = ConveyorDiretion.Up;
        }
        else if (Parent.Direction.z > 0.0)
        {
            ConveyorDirection = ConveyorDiretion.Right;
        }
        else if (Parent.Direction.z < 0.0)
        {
            ConveyorDirection = ConveyorDiretion.Left;
        }
    }

    private MyGameObject[] GetInputs()
    {
        Vector3Int[] positions = new Vector3Int[]
        {
            Parent.PositionGrid,
            Parent.PositionGrid + new Vector3Int(1, 0, 0),
        };

        switch (ConveyorDirection)
        {
            case ConveyorDiretion.Down:
                positions[0].x -= 1;
                positions[1].x -= 1;
                break;

            case ConveyorDiretion.Up:
                positions[0].x += 1;
                positions[1].x += 1;
                break;

            case ConveyorDiretion.Right:
                positions[0].z -= 1;
                positions[1].z -= 1;
                break;

            case ConveyorDiretion.Left:
                positions[0].z += 1;
                positions[1].z += 1;
                break;

            default:
                return null;
        }

        return GetGameObjects(positions);
    }

    private MyGameObject[] GetOutputs()
    {
        Vector3Int[] positions = new Vector3Int[]
        {
            Parent.PositionGrid,
            Parent.PositionGrid + new Vector3Int(1, 0, 0),
        };

        switch (ConveyorDirection)
        {
            case ConveyorDiretion.Down:
                positions[0].x += 1;
                positions[1].x += 1;
                break;

            case ConveyorDiretion.Up:
                positions[0].x -= 1;
                positions[1].x -= 1;
                break;

            case ConveyorDiretion.Right:
                positions[0].z += 1;
                positions[1].z += 1;
                break;

            case ConveyorDiretion.Left:
                positions[0].z -= 1;
                positions[1].z -= 1;
                break;

            default:
                return null;
        }

        return GetGameObjects(positions);
    }

    private MyGameObject[] GetGameObjects(Vector3Int[] positions)
    {
        MyGameObject[] myGameObjects = new MyGameObject[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            if (Map.Instance.Cells[positions[i].x, positions[i].z].Occupied.TryGetValue(Parent.Player, out ValueGameObjects value) == false) // TODO: Check index range.
            {
                myGameObjects[i] = null;
            }
            else if (value.GameObjects.Count <= 0)
            {
                myGameObjects[i] = null;
            }
            else
            {
                myGameObjects[i] = value.GameObjects.First();
            }
        }

        return myGameObjects;
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
    private ConveyorDiretion ConveyorDirection = ConveyorDiretion.Right;

    [SerializeField]
    private ConveyorState ConveyorState = ConveyorState.Receiving;

    // private int activeInputIndex = 0;
    // private int activeOutputIndex = 0;

    private Transform[] Item = new Transform[2];
}
