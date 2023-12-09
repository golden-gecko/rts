using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour
{
    public Bounds Bounds
    {
        get
        {
            Quaternion rotation = Utils.ResetRotation(transform);

            Bounds bounds = BoundsOriented;

            Utils.RestoreRotation(transform, rotation);

            return bounds;
        }
    }

    public Bounds BoundsOriented
    {
        get
        {
            Bounds bounds = new Bounds(Position, Vector3.zero);

            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                bounds.Encapsulate(collider.bounds);
            }

            return bounds;
        }
    }

    public BoundsInt BoundsGrid
    {
        get
        {
            BoundsInt boundsGrid = new BoundsInt();

            boundsGrid.min = Utils.ToGrid(Bounds.min, Config.Map.Scale);
            boundsGrid.max = Utils.ToGrid(Bounds.max, Config.Map.Scale);

            return boundsGrid;
        }
    }

    public Vector3 Center { get => new Vector3(Position.x, Position.y + Bounds.extents.y, Position.z); }

    public Vector3 Direction { get => transform.forward; }

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public Vector3Int PositionGrid { get => Utils.ToGrid(BoundsOriented.min, Config.Map.Scale); }

    public float Radius { get => Mathf.Max(Mathf.Max(Bounds.extents.x, Bounds.extents.y), Bounds.extents.z); }

    public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }

    public Vector3 Scale { get => transform.localScale; }

    public Vector3 Size { get => Bounds.size; }

    public Vector3Int SizeGrid { get => Utils.ToGrid(Bounds.size, Config.Map.Scale); }
}
