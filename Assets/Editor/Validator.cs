using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Validator : EditorWindow
{
    [MenuItem("Tools/Validate", false, 3)]
    public static void Validate()
    {
        Tools.ClearLog();

        IEnumerable<GameObject> prefabs = Tools.GetPrefabs();

        ValidateConstructionResources(prefabs);
        ValidateNames(prefabs);
        ValidatePhysics(prefabs);
        ValidateProperties(prefabs);
        ValidateStorage(prefabs);

        GameObject[] gameObjects = Tools.GetGameObjects();

        ValidateGameObjects(gameObjects);
    }

    private static void ValidateConstructionResources(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent(out Part part))
            {
                if (part.ConstructionResources.Items.Count <= 0)
                {
                    Debug.Log(string.Format("Resource {0} has invalid construction resource count ({1}).", part.name, part.ConstructionResources.Items.Count));
                }

                foreach (Resource resource in part.ConstructionResources.Items)
                {
                    if (resource.Current <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid construction resource current value ({1}).", part.name, part.ConstructionResources.Items.Count));
                    }

                    if (resource.Max <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid construction resource max value ({1}).", part.name, part.ConstructionResources.Items.Count));
                    }
                }
            }
        }
    }

    private static void ValidateGameObjects(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent(out MyGameObject myGameObject))
            {
                if (myGameObject.Player == null)
                {
                    Debug.Log(string.Format("Game object {0} has no player.", gameObject.name));
                }
            }
        }
    }

    private static void ValidateNames(IEnumerable<GameObject> gameObjects)
    {
        Dictionary<string, int> names = new Dictionary<string, int>();

        foreach (GameObject gameObject in gameObjects)
        {
            if (names.TryGetValue(gameObject.name, out int _))
            {
                names[gameObject.name] += 1;
            }
            else
            {
                names[gameObject.name] = 1;
            }
        }

        IEnumerable<KeyValuePair<string, int>> duplicates = names.Where(x => x.Value > 1);

        if (duplicates.Count() > 0)
        {
            Debug.Log(string.Format("Some game objects have duplicates names: {0}", string.Join(", ", names.Where(x => x.Value > 1))));
        }
    }

    private static void ValidatePhysics(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
            {
                if (collider.isTrigger)
                {
                    Debug.Log(string.Format("Resource {0} has collider trigger enabled ({1}).", gameObject.name, collider.isTrigger));
                }
            }

            foreach (Rigidbody rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
            {
                if (rigidbody.isKinematic == false)
                {
                    Debug.Log(string.Format("Resource {0} is not kinematic ({1}).", gameObject.name, rigidbody.isKinematic));
                }

                if (rigidbody.useGravity)
                {
                    Debug.Log(string.Format("Resource {0} is using gravity ({1}).", gameObject.name, rigidbody.useGravity));
                }
            }
        }
    }

    private static void ValidateProperties(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent(out MyGameObject myGameObject))
            {
                bool isDisaster = myGameObject.TryGetComponent(out Disaster _);
                bool isMissile = myGameObject.TryGetComponent(out Missile _);

                if (isDisaster == false && isMissile == false && myGameObject.DestroyEffect == null)
                {
                    Debug.Log(string.Format("Resource {0} has no destroy effect.", gameObject.name));
                }

                if ((isDisaster || isMissile) && myGameObject.DestroyEffect != null)
                {
                    Debug.Log(string.Format("Resource {0} has destroy effect.", gameObject.name));
                }

                foreach (string skill in myGameObject.Skills.Keys)
                {
                    if (skill.Length <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid skill name ({1}).", gameObject.name, skill));
                    }
                }
            }
        }
    }

    private static void ValidateStorage(IEnumerable<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            foreach (Storage storage in gameObject.GetComponentsInChildren<Storage>())
            {
                foreach (Resource resource in storage.Resources.Items)
                {
                    if (resource.Name.Length <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid resource name ({1}).", gameObject.name, resource));
                    }

                    if (resource.Current < 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid current value ({1}, {2}).", gameObject.name, resource.Name, resource.Current));
                    }

                    if (resource.Max <= 0)
                    {
                        Debug.Log(string.Format("Resource {0} has invalid max value ({1}, {2}).", gameObject.name, resource.Name, resource.Max));
                    }
                }
            }
        }
    }
}
