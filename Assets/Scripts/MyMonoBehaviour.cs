using UnityEngine;

public class MyMonoBehaviour : MonoBehaviour
{
    public Vector3 Center { get => new Vector3(Position.x, Position.y + Size.y / 2.0f, Position.z); }

    public Vector3 Direction { get => transform.forward; }

    public Vector3 Position { get => transform.position; set => transform.position = value; }

    public float Radius { get => (Size.x + Size.y + Size.z) / 3.0f; } // TODO: Fix. Use max?

    public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }

    public Vector3 Scale { get => transform.localScale; }

    public Vector3 Size
    {
        get
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            if (colliders.Length <= 0)
            {
                return Vector3.zero;
            }

            Quaternion rotation = Utils.ResetRotation(transform);

            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (Collider collider in colliders)
            {
                min.x = Mathf.Min(min.x, collider.bounds.min.x);
                min.y = Mathf.Min(min.y, collider.bounds.min.y);
                min.z = Mathf.Min(min.z, collider.bounds.min.z);

                max.x = Mathf.Max(max.x, collider.bounds.max.x);
                max.y = Mathf.Max(max.y, collider.bounds.max.y);
                max.z = Mathf.Max(max.z, collider.bounds.max.z);
            }

            Utils.RestoreRotation(transform, rotation);

            return max - min;
        }
    }

    public Vector3Int SizeGrid
    {
        get
        {
            Vector3 size = Size;

            int x = Mathf.CeilToInt(size.x / Config.Map.Scale);
            int z = Mathf.CeilToInt(size.z / Config.Map.Scale);

            return new Vector3Int(x, 0, z);
        }
    }

    public Vector3Int PositionMinGrid { get => Utils.ToGrid(Position - Size / 2.0f, Config.Map.Scale); }

    public Vector3Int PositionMaxGrid { get => Utils.ToGrid(Position + Size / 2.0f, Config.Map.Scale) - new Vector3Int(1, 0, 1); }
}
