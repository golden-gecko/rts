using UnityEngine;
using static UnityEngine.UI.Image;

public class Utils
{
    public static MyGameObject CreateGameObject(string prefab, Vector3 position, Quaternion rotation, Player player, MyGameObjectState state)
    {
        return CreateGameObject(Resources.Load<MyGameObject>(prefab), position, rotation, player, state);
    }

    public static MyGameObject CreateGameObject(MyGameObject resource, Vector3 position, Quaternion rotation, Player player, MyGameObjectState state)
    {
        MyGameObject myGameObject = Object.Instantiate(resource, position, rotation);

        myGameObject.SetPlayer(player);
        myGameObject.SetState(state);

        if (state == MyGameObjectState.Cursor)
        {
            foreach (Collider collider in myGameObject.GetComponents<Collider>())
            {
                collider.enabled = false;
            }

            foreach (MyComponent myComponent in myGameObject.GetComponents<MyComponent>())
            {
                myComponent.enabled = false;
            }

            foreach (MyGameObject i in myGameObject.GetComponents<MyGameObject>())
            {
                i.enabled = false;
            }
        }

        return myGameObject;
    }

    public static bool IsCloseTo(Vector3 source, Vector3 target)
    {
        return IsInRange(source, target, 0.0f, 1.0f);
    }

    public static bool IsInRange(Vector3 source, Vector3 target, float rangeMax)
    {
        return IsInRange(source, target, 0.0f, rangeMax);
    }

    public static bool IsInRange(Vector3 source, Vector3 target, float rangeMin, float rangeMax)
    {
        Vector3 a = source;
        Vector3 b = target;

        a.y = 0.0f;
        b.y = 0.0f;

        float magnitude = (b - a).magnitude;

        return rangeMin <= magnitude && magnitude <= rangeMax;
    }

    public static bool IsTerrain(RaycastHit hit)
    {
        return hit.transform.CompareTag("Terrain");
    }

    public static bool IsWater(RaycastHit hit)
    {
        return hit.transform.CompareTag("Water");
    }

    public static Quaternion ResetRotation(MyGameObject myGameObject)
    {
        Quaternion rotation = myGameObject.Rotation;
        myGameObject.Rotation = Quaternion.identity;
        Physics.SyncTransforms();

        return rotation;
    }

    public static void RestoreRotation(MyGameObject myGameObject, Quaternion rotation)
    {
        myGameObject.Rotation = rotation;
        Physics.SyncTransforms();
    }

    public static Vector3 SnapToGrid(Vector3 position)
    {
        float scale = Config.TerrainConstructionScale;

        float x = Mathf.Floor(position.x / scale) * scale + scale / 2.0f;
        float z = Mathf.Floor(position.z / scale) * scale + scale / 2.0f;

        return new Vector3(x, position.y, z);
    }

    public static bool RaycastFromMouse(out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, layerMask);
    }

    public static RaycastHit[] RaycastAllFromMouse(int layerMask = Physics.DefaultRaycastLayers)
    {
        return RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), layerMask);
    }

    public static bool RaycastFromTop(Vector3 position, out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Raycast(new Ray(new Vector3(position.x, Config.TerrainMaxHeight, position.z), Vector3.down), out hitInfo, layerMask);
    }

    public static RaycastHit[] RaycastAllFromTop(Vector3 position, int layerMask = Physics.DefaultRaycastLayers)
    {
        return RaycastAll(new Ray(new Vector3(position.x, Config.TerrainMaxHeight, position.z), Vector3.down), layerMask);
    }

    public static bool Raycast(Ray ray, out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.Raycast(ray, out hitInfo, float.MaxValue, layerMask);
    }

    public static RaycastHit[] RaycastAll(Ray ray, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.RaycastAll(ray, float.MaxValue, layerMask);
    }
}
