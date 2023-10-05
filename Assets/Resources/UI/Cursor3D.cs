using UnityEngine;

public class Cursor3D : MonoBehaviour
{
    public static Cursor3D Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (GameObject == null)
        {
            return;
        }

        // Rotate.
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
        {
            GameObject.transform.Rotate(Vector3.up, Config.Cursor3D.RotateStep);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            GameObject.transform.Rotate(Vector3.down, Config.Cursor3D.RotateStep);
        }

        // Follow mouse.
        if (Map.Instance.MouseToPosition(GameObject, out Vector3 position, out _))
        {
            GameObject.Position = Config.Cursor3D.SnapToGrid ? Utils.SnapToGrid(position) : position;
        }

        // Verify position.
        if (GameObject.HasCorrectPosition())
        {
            GameObject.Indicators.OnErrorEnd();
        }
        else
        {
            GameObject.Indicators.OnError();
        }
    }

    public void Create(string prefab, Player player)
    {
        GameObject = Utils.CreateGameObject(prefab, Vector3.zero, Quaternion.identity, player, MyGameObjectState.Cursor);
        GameObject.gameObject.layer = LayerMask.NameToLayer("Cursor");
    }

    public void Destroy()
    {
        Destroy(GameObject.gameObject);
    }

    public bool HasCorrectPosition()
    {
        return GameObject.HasCorrectPosition();
    }

    public Vector3 Position { get => GameObject.Position; }

    public Quaternion Rotation { get => GameObject.Rotation; }

    public bool Visible { get => GameObject != null; }

    private MyGameObject GameObject { get; set; }
}
