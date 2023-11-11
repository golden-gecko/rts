using System.IO;
using UnityEngine;

public class Utils
{
    #region Enum
    public static string[] GetFormationNames()
    {
        string[] names = System.Enum.GetNames(typeof(Formation));

        System.Array.Sort(names);

        return names;
    }

    public static string[] GetOrderNames()
    {
        string[] names = System.Enum.GetNames(typeof(OrderType));

        System.Array.Sort(names);

        return names;
    }
    #endregion

    #region Grid
    public static Vector3 SnapToCorner(Vector3 position, float scale)
    {
        float x = Mathf.Floor(position.x / scale) * scale;
        float z = Mathf.Floor(position.z / scale) * scale;

        return new Vector3(x, position.y, z);
    }

    public static Vector3 SnapToCenter(Vector3 position, float scale)
    {
        float x = Mathf.Floor(position.x / scale) * scale + scale / 2.0f;
        float z = Mathf.Floor(position.z / scale) * scale + scale / 2.0f;

        return new Vector3(x, position.y, z);
    }

    public static Vector3Int ToGrid(Vector3 position, float scale)
    {
        int x = Mathf.FloorToInt(position.x / scale);
        int z = Mathf.FloorToInt(position.z / scale);

        return new Vector3Int(x, 0, z);
    }
    #endregion

    #region Instantiate
    public static MyGameObject CreateGameObject(string prefab, Vector3 position, Quaternion rotation, Player player, MyGameObjectState state)
    {
        Game.Instance.GetGameObject(prefab, out MyGameObject myGameObject, out Blueprint blueprint);

        if (myGameObject != null)
        {
            return CreateGameObject(myGameObject, position, rotation, player, state);
        }

        if (blueprint != null)
        {
            return CreateGameObject(blueprint, position, rotation, player, state);
        }

        return null;
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

            foreach (Part myComponent in myGameObject.GetComponents<Part>())
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

    public static MyGameObject CreateGameObject(Blueprint blueprint, Vector3 position, Quaternion rotation, Player player, MyGameObjectState state)
    {
        MyGameObject main = null;

        foreach (BlueprintComponent part in blueprint.Parts)
        {
            GameObject partResource = LoadPart(part.PartType, part.Name);

            if (partResource == null)
            {
                continue;
            }

            GameObject partInstance = Object.Instantiate(partResource, position, rotation);

            if (partInstance == null)
            {
                continue;
            }

            if (partInstance.TryGetComponent(out MyGameObject myGameObject))
            {
                main = myGameObject;
                main.SetPlayer(player);
                main.SetState(state);
            }
        }

        if (main != null && state == MyGameObjectState.Cursor)
        {
            foreach (Collider collider in main.GetComponents<Collider>())
            {
                collider.enabled = false;
            }

            foreach (Part myComponent in main.GetComponents<Part>())
            {
                myComponent.enabled = false;
            }

            foreach (MyGameObject i in main.GetComponents<MyGameObject>())
            {
                i.enabled = false;
            }
        }

        return main;
    }

    public static GameObject LoadPart(PartType partType, string name)
    {
        switch (partType)
        {
            case PartType.Chassis:
                return Game.Instance.Config.Chassis.Find(x => x.name == name);

            case PartType.Drive:
                return Game.Instance.Config.Drives.Find(x => x.name == name);

            case PartType.Gun:
                return Game.Instance.Config.Guns.Find(x => x.name == name);
        }

        return null;
    }
    #endregion

    #region Mask
    public static int GetGameObjectMask()
    {
        return LayerMask.GetMask("GameObject");
    }

    public static int GetMapMask()
    {
        return LayerMask.GetMask("Terrain") | LayerMask.GetMask("Water");
    }
    #endregion

    #region Math
    public static float Angle(Vector3 value)
    {
        float angle = Vector3.Angle(value, Vector3.forward);

        return (Vector3.Angle(Vector3.right, value) > 90.0f) ? 360.0f - angle : angle;
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

    public static bool IsPointInCircle(int x, int z, Vector3Int center, int radius)
    {
        int dx = Mathf.Abs(x - center.x);
        int dz = Mathf.Abs(z - center.z);

        if (dx + dz < radius)
        {
            return true;
        }

        if (dx > radius)
        {
            return false;
        }

        if (dz > radius)
        {
            return false;
        }

        return dx * dx + dz * dz < radius * radius;
    }

    public static bool IsPointInRect(int x, int z, int size)
    {
        if (x < 0 || x > size - 1)
        {
            return false;
        }

        if (z < 0 || z > size - 1)
        {
            return false;
        }

        return true;
    }

    public static int MakeOdd(int value)
    {
        return value % 2 == 0 ? value + 1 : value;
    }
    #endregion

    #region Raycast
    public static MyGameObject RaycastGameObjectFromMouse()
    {
        RaycastHit hitInfo;

        if (Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, GetGameObjectMask()) == false)
        {
            return null;
        }

        MyGameObject myGameObject = GetGameObject(hitInfo);

        if (myGameObject == null)
        {
            return null;
        }

        if (myGameObject.VisibilityState != MyGameObjectVisibilityState.Visible)
        {
            return null;
        }

        return myGameObject;
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
        return Raycast(new Ray(new Vector3(position.x, Config.Map.MaxHeight, position.z), Vector3.down), out hitInfo, layerMask);
    }

    public static RaycastHit[] RaycastAllFromTop(Vector3 position, int layerMask = Physics.DefaultRaycastLayers)
    {
        return RaycastAll(new Ray(new Vector3(position.x, Config.Map.MaxHeight, position.z), Vector3.down), layerMask);
    }

    public static bool Raycast(Ray ray, out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.Raycast(ray, out hitInfo, float.MaxValue, layerMask);
    }

    public static RaycastHit[] RaycastAll(Ray ray, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.RaycastAll(ray, float.MaxValue, layerMask);
    }

    public static RaycastHit[] SphereCastAll(Vector3 position, float range, int layerMask = Physics.DefaultRaycastLayers)
    {
        return Physics.SphereCastAll(position, range, Vector3.down, 0.0f, layerMask);
    }
    #endregion

    #region Resources
    public static Texture2D LoadPortrait(string name)
    {
        return Resources.Load<Texture2D>(Path.Join("Portraits", name));
    }
    #endregion

    #region Rotation
    public static Quaternion ResetRotation(Transform transform)
    {
        Quaternion rotation = transform.rotation;
        transform.rotation = Quaternion.identity;
        Physics.SyncTransforms();

        return rotation;
    }

    public static Quaternion ResetRotation(MyGameObject myGameObject)
    {
        Quaternion rotation = myGameObject.Rotation;
        myGameObject.Rotation = Quaternion.identity;
        Physics.SyncTransforms();

        return rotation;
    }

    public static void RestoreRotation(Transform transform, Quaternion rotation)
    {
        transform.rotation = rotation;
        Physics.SyncTransforms();
    }

    public static void RestoreRotation(MyGameObject myGameObject, Quaternion rotation)
    {
        myGameObject.Rotation = rotation;
        Physics.SyncTransforms();
    }
    #endregion

    #region String
    public static string FormatName(string name)
    {
        string formatted = string.Empty;

        foreach (char c in name)
        {
            if (c >= 'A' && c <= 'Z')
            {
                formatted += ' ';
                formatted += c;
            }
            else if (c == '_')
            {
                formatted += ' ';
            }
            else
            {
                formatted += c;
            }
        }

        return formatted.Trim();
    }
    #endregion

    public static Color32[] CreateColorArray(int width, int height, byte r, byte g, byte b, byte a)
    {
        Color32[] colors = new Color32[width * height];

        System.Array.Fill(colors, new Color32(r, g, b, a));

        return colors;
    }

    public static MyGameObject GetGameObject(Collision collision)
    {
        return collision.collider.GetComponentInParent<MyGameObject>();
    }

    public static MyGameObject GetGameObject(RaycastHit hitInfo)
    {
        return hitInfo.transform.GetComponentInParent<MyGameObject>();
    }

    public static bool IsTerrain(Collision collision)
    {
        return collision.collider.CompareTag("Terrain");
    }

    public static bool IsWater(Collision collision)
    {
        return collision.collider.CompareTag("Water");
    }

    public static bool IsTerrain(RaycastHit hitInfo)
    {
        return hitInfo.transform.CompareTag("Terrain");
    }

    public static bool IsWater(RaycastHit hitInfo)
    {
        return hitInfo.transform.CompareTag("Water");
    }
}
